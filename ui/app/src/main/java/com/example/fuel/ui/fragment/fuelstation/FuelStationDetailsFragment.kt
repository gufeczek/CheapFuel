package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.ProgressBar
import android.widget.TextView
import androidx.appcompat.widget.LinearLayoutCompat
import androidx.core.view.get
import androidx.core.view.size
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationDetailsBinding
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationLocation
import com.example.fuel.model.FuelType
import com.example.fuel.utils.calculateDistance
import com.example.fuel.utils.converters.UnitConverter
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.bottomsheet.BottomSheetDialogFragment


class FuelStationDetailsFragment : BottomSheetDialogFragment() {
    private lateinit var binding: FragmentFuelStationDetailsBinding
    private lateinit var viewModel: FuelStationDetailsViewModel
    private lateinit var fuelStationDetailsView: View

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelStationDetailsBinding.inflate(inflater, container, false)
        fuelStationDetailsView = inflater.inflate(R.layout.fragment_fuel_station_details, container, false)

        val fuelStationId = requireArguments().getLong("fuelStationId")
        loadData(fuelStationId)

        return fuelStationDetailsView
    }

    private fun loadData(fuelStationId: Long) {
        viewModel.getFuelStationDetails(fuelStationId)
        viewModel.fuelStationDetails.observe(viewLifecycleOwner) { response ->
            val fuelStationData = response.body()

            populateViewWithData(fuelStationData!!)
            showLayout()
            addFuelPriceCards(fuelStationData.fuelTypes)
        }
    }

    private fun populateViewWithData(fuelStation: FuelStationDetails) {
        fuelStationDetailsView.findViewById<TextView>(R.id.tv_stationChainName).text = fuelStation.name ?: fuelStation.stationChain.name
        fuelStationDetailsView.findViewById<TextView>(R.id.tv_fuelStationAddress).text = fuelStation.address.toString()
        setDistance(fuelStation.location)
    }

    private fun setDistance(fuelStationLocation: FuelStationLocation) {
        val textView = fuelStationDetailsView.findViewById<TextView>(R.id.tv_distanceBetweenUserAndStation)

        if (!isGpsEnabled(requireContext())) {
            textView.visibility = View.GONE
            return
        }

        val location = getUserLocation(requireContext()) ?: return
        val distance = calculateDistance(location.latitude, location.longitude, fuelStationLocation.latitude, fuelStationLocation.longitude)

        textView.text =  resources.getString(R.string.from_you, UnitConverter.fromMetersToTarget(distance.toDouble()))
        textView.visibility = View.VISIBLE
    }

    private fun addFuelPriceCards(fuelTypes: Array<FuelType>) {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        val parent = fuelStationDetailsView.findViewById<LinearLayoutCompat>(R.id.llc_fuelPriceContainer)
        var llc: LinearLayoutCompat? = null

        for (i in fuelTypes.indices) {
            if (i % 2 == 0) {
                llc = createEmptyRow()
                parent.addView(llc)
            }

            val fuelPriceCardFragment = FuelPriceCardFragment()
            fragmentTransaction.add(llc!!.id, fuelPriceCardFragment)
        }

        fragmentTransaction.commitNow()

//        if (llc != null && fuelTypes.size % 2 == 1) {
//            val priceCard = llc[llc.size - 1]
//            val params = LinearLayoutCompat.LayoutParams(parent.width / 2, priceCard.height, 0.5F)
//            priceCard.layoutParams = params
//            priceCard.requestLayout()
//            llc.requestLayout()
//        }
    }

    private fun createEmptyRow(): LinearLayoutCompat {
        val llc = LinearLayoutCompat(requireContext())
        llc.id = View.generateViewId()
        llc.orientation = LinearLayoutCompat.HORIZONTAL
        return llc
    }

    private fun createSpacer(): View {
        return LayoutInflater.from(requireContext())
            .inflate(R.layout.horizontal_spacer_50, null, false) as View
    }

    private fun showLayout() {
        val layout = fuelStationDetailsView.findViewById<LinearLayoutCompat>(R.id.ll_mainFuelDetailsLayout)
        layout.visibility = View.VISIBLE

        val progressSpinner = fuelStationDetailsView.findViewById<ProgressBar>(R.id.fuel_station_details_loading_spinner)
        progressSpinner.visibility = View.GONE
    }

    private fun hideLayout() {
        val layout = fuelStationDetailsView.findViewById<LinearLayoutCompat>(R.id.ll_mainFuelDetailsLayout)
        layout.visibility = View.GONE

        val progressSpinner = fuelStationDetailsView.findViewById<ProgressBar>(R.id.fuel_station_details_loading_spinner)
        progressSpinner.visibility = View.VISIBLE
    }

    override fun onDestroyView() {
        super.onDestroyView()
        viewModel.clear()
        hideLayout()
    }

    companion object {
        const val TAG = "FuelStationDetails"
    }
}