package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ProgressBar
import android.widget.TextView
import androidx.appcompat.widget.LinearLayoutCompat
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationDetailsBinding
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationLocation
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