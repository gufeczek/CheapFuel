package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ProgressBar
import android.widget.TextView
import androidx.appcompat.widget.LinearLayoutCompat
import androidx.core.widget.NestedScrollView
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationDetailsBinding
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationLocation
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelTypeWithPrice
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.utils.calculateDistance
import com.example.fuel.utils.converters.UnitConverter
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup


class FuelStationDetailsFragment : BottomSheetDialogFragment() {
    private lateinit var binding: FragmentFuelStationDetailsBinding
    private lateinit var viewModel: FuelStationDetailsViewModel
    private lateinit var fuelStationDetailsView: View
    private var fuelStationId: Long? = null

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelStationDetailsBinding.inflate(inflater, container, false)
        fuelStationDetailsView = inflater.inflate(R.layout.fragment_fuel_station_details, container, false)

        fuelStationId = requireArguments().getLong("fuelStationId")
        loadData()
        Log.d("fuelStation", fuelStationId.toString())
        return fuelStationDetailsView
    }

    private fun loadData() {
        viewModel.getFuelStationDetails(fuelStationId!!)
        viewModel.fuelStationDetails.observe(viewLifecycleOwner) { response ->
            val fuelStationData = response.body()

            populateViewWithData(fuelStationData!!)
            showLayout()
            addFuelPriceCards(fuelStationData.fuelTypes)
            addFuelStationServices(fuelStationData.services)
            initCommentObserver()
            initCommentSection()
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

    private fun addFuelPriceCards(fuelTypes: Array<FuelTypeWithPrice>) {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        val parent = fuelStationDetailsView.findViewById<LinearLayoutCompat>(R.id.llc_fuelPriceContainer)
        var llc: LinearLayoutCompat? = null

        for (i in fuelTypes.indices) {
            if (i % 2 == 0) {
                llc = createEmptyRow()
                parent.addView(llc)
            }

            val fuelPriceCardFragment = FuelPriceCardFragment(fuelTypes[i])
            fragmentTransaction.add(llc!!.id, fuelPriceCardFragment)
        }

        fragmentTransaction.commitNow()
    }

    private fun createEmptyRow(): LinearLayoutCompat {
        val llc = LinearLayoutCompat(requireContext())
        llc.id = View.generateViewId()
        llc.orientation = LinearLayoutCompat.HORIZONTAL
        return llc
    }

    private fun addFuelStationServices(services: Array<FuelStationService>) {
        val serviceContainer = fuelStationDetailsView.findViewById<ChipGroup>(R.id.cg_fuelStationServicesContainer);

        for (service in services) {
            val chip = createServiceChip(service)
            serviceContainer.addView(chip)
        }

        if (!viewModel.hasAnyServices()) hideFuelStationServicesSection()
    }

    private fun hideFuelStationServicesSection() {
        val servicesSection = fuelStationDetailsView.findViewById<LinearLayoutCompat>(R.id.llc_servicesSection)
        servicesSection.visibility = View.GONE

        val serviceSectionSpacer = fuelStationDetailsView.findViewById<View>(R.id.v_serviceSectionSpacer)
        serviceSectionSpacer.visibility = View.GONE
    }

    private fun createServiceChip(service: FuelStationService): Chip {
        val chip = LayoutInflater.from(requireContext()).inflate(R.layout.filter_chip, null, false) as Chip
        initChipAppearance(chip, requireContext())
        chip.text = service.name
        chip.isChecked = false
        chip.isEnabled = false
        return chip
    }

    private fun initCommentSection() {
        loadComments()

        fuelStationDetailsView.findViewById<NestedScrollView>(R.id.nsv_fuel_details_bottom_sheet)
            .setOnScrollChangeListener { v, scrollX, scrollY, oldScrollX, oldScrollY ->
                val nsv = v as NestedScrollView

                if (oldScrollY < scrollY
                    && scrollY == (nsv.getChildAt(0).measuredHeight - nsv.measuredHeight)
                    && viewModel.hasMoreReviews()) {

                    loadComments()
                }
            }
    }

    private fun initCommentObserver() {
        viewModel.fuelStationReviews.observe(viewLifecycleOwner) { response ->
            val fragmentManager = childFragmentManager
            val fragmentTransaction = fragmentManager.beginTransaction()
            val parent = fuelStationDetailsView.findViewById<LinearLayoutCompat>(R.id.llc_commentsContainer)

            val page = response.body()

            for (comment in page?.data!!) {
                val commentFragment = FuelStationCommentFragment(comment)
                fragmentTransaction.add(parent.id, commentFragment)
            }
            fragmentTransaction.commitNow()

            if (!viewModel.hasMoreReviews()) hideCommentSectionProgressBar()
        }
    }

    private fun loadComments() {
        viewModel.getNextPageOfFuelStationReviews(fuelStationId!!)
    }

    private fun hideCommentSectionProgressBar() {
        val progressBar = fuelStationDetailsView.findViewById<ProgressBar>(R.id.pb_comment_load)
        progressBar.visibility = View.GONE
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