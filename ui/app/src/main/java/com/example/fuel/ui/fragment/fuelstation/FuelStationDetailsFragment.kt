package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.appcompat.widget.LinearLayoutCompat
import androidx.core.widget.NestedScrollView
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationDetailsBinding
import com.example.fuel.utils.Auth
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationLocation
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelTypeWithPrice
import com.example.fuel.model.review.Review
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.utils.calculateDistance
import com.example.fuel.utils.converters.UnitConverter
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import com.google.android.material.chip.Chip
import com.google.android.material.dialog.MaterialAlertDialogBuilder


class FuelStationDetailsFragment : BottomSheetDialogFragment() {
    private lateinit var binding: FragmentFuelStationDetailsBinding
    private lateinit var viewModel: FuelStationDetailsViewModel
    private var fuelStationId: Long? = null

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelStationDetailsBinding.inflate(inflater, container, false)

        fuelStationId = requireArguments().getLong("fuelStationId")

        loadFuelStationData()
        initDeleteFuelStationObserver()
        initReviewObserver()
        initUserReview()
        initReviewSection()
        initAddReviewButton()
        initNewReviewObserver()
        initEditedReviewObserver()
        initDeleteUserReviewObserver()
        initDeleteDiffUserReviewObserver()
        initUserFavouriteObserver()
        initAddToFavouriteObserver()
        initRemoveFavouriteObserver()
        initNewFuelPriceObserver()
        initReportReviewObserver()

        return binding.root
    }

    private fun loadFuelStationData() {
        viewModel.getFuelStationDetails(fuelStationId!!)
        viewModel.fuelStationDetails.observe(viewLifecycleOwner) { response ->
            val fuelStationData = response.body()

            populateViewWithData(fuelStationData!!)
            initUserSpecificLayout()
            showLayout()
            addFuelPriceCards(fuelStationData.fuelTypes)
            addFuelStationServices(fuelStationData.services)
            initEditFuelPriceButton()
        }
    }

    private fun populateViewWithData(fuelStation: FuelStationDetails) {
        binding.tvStationChainName.text = fuelStation.name ?: fuelStation.stationChain.name
        binding.tvFuelStationAddress.text = fuelStation.address.toString()
        setDistance(fuelStation.location)
    }

    private fun setDistance(fuelStationLocation: FuelStationLocation) {
        val textView = binding.tvDistanceBetweenUserAndStation

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

        val parent = binding.llcFuelPriceContainer
        var llc: LinearLayoutCompat? = null

        parent.removeAllViews()

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
        val serviceContainer = binding.cgFuelStationServicesContainer
        serviceContainer.removeAllViews()

        for (service in services) {
            val chip = createServiceChip(service)
            serviceContainer.addView(chip)
        }

        if (!viewModel.hasAnyServices()) hideFuelStationServicesSection()
    }

    private fun hideFuelStationServicesSection() {
        binding.llcServicesSection.visibility = View.GONE
        binding.vServiceSectionSpacer.visibility = View.GONE
    }

    private fun createServiceChip(service: FuelStationService): Chip {
        val chip = LayoutInflater.from(requireContext()).inflate(R.layout.filter_chip, null, false) as Chip
        initChipAppearance(chip, requireContext())
        chip.text = service.name
        chip.isChecked = false
        chip.isEnabled = false
        return chip
    }

    private fun initUserSpecificLayout() {
        if (viewModel.isAdmin()) {
            initDeleteFuelStationButton()
        }

        if (viewModel.isFuelStationOwner() || viewModel.isAdmin()) {
            initEditFuelStationButton()
        }
    }

    private fun initEditFuelStationButton() {
        val editFuelStationButton = binding.acibEditFuelStation
        editFuelStationButton.visibility = View.VISIBLE

        editFuelStationButton.setOnClickListener {
            val fuelStationEditorFragment = FuelStationEditorFragment()
            fuelStationEditorFragment.show(requireFragmentManager(), FuelStationEditorFragment.TAG)
        }
    }

    private fun initDeleteFuelStationButton() {
        val deleteFuelStationButton = binding.acibDeleteFuelStation
        deleteFuelStationButton.visibility = View.VISIBLE

        deleteFuelStationButton.setOnClickListener {
            askForDeleteConfirmationOfFuelStation()
        }
    }

    private fun askForDeleteConfirmationOfFuelStation() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setMessage(getString(R.string.fuel_station_ask_if_delete))
            .setPositiveButton(resources.getString(R.string.yes)) { _, _ ->
                viewModel.deleteFuelStation(fuelStationId!!)
            }
            .setNegativeButton(resources.getString(R.string.no), null)
            .show()
    }

    private fun initEditFuelPriceButton() {
        if (!shouldAllowToEditFuelPrice()) return

        val editFuelPriceButton = binding.acibEditFuelPrice
        editFuelPriceButton.visibility = View.VISIBLE

        editFuelPriceButton.setOnClickListener {
            val fuelPriceEditorFragment = FuelPriceEditorFragment()
            fuelPriceEditorFragment.show(requireFragmentManager(), FuelPriceEditorFragment.TAG)
        }
    }

    private fun shouldAllowToEditFuelPrice(): Boolean {
        return viewModel.isAdmin() ||
                viewModel.isFuelStationOwner() ||
                (isGpsEnabled(requireContext()) && viewModel.isCloseToFuelStation(getUserLocation(requireContext())))
    }

    private fun initReviewSection() {
        loadReviews()

        binding.nsvFuelDetailsBottomSheet
            .setOnScrollChangeListener { v, _, scrollY, _, oldScrollY ->
                val nsv = v as NestedScrollView

                if (oldScrollY < scrollY
                    && scrollY == (nsv.getChildAt(0).measuredHeight - nsv.measuredHeight)
                    && viewModel.hasMoreReviews()) {

                    loadReviews()
                }
            }
    }

    private fun loadReviews() {
        viewModel.getNextPageOfFuelStationReviews(fuelStationId!!)
    }

    private fun initDeleteFuelStationObserver() {
        viewModel.deleteFuelStation.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                dismiss()
            }

            val text = if (response.isSuccessful) resources.getString(R.string.deleted)
            else resources.getString(R.string.an_error_occurred)

            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initReviewObserver() {
        viewModel.fuelStationReviews.observe(viewLifecycleOwner) { response ->
            val fragmentManager = childFragmentManager
            val fragmentTransaction = fragmentManager.beginTransaction()
            val parent = binding.llcReviewsContainer

            val page = response.body()

            for (review in page?.data!!) {
                if (review.username == Auth.username) continue

                val reviewFragment = FuelStationReviewFragment(review)
                fragmentTransaction.add(parent.id, reviewFragment)
            }
            fragmentTransaction.commitNow()

            if (!viewModel.hasMoreReviews()) hideReviewSectionProgressBar()
        }
    }

    private fun initNewReviewObserver() {
        viewModel.newUserReview.observe(viewLifecycleOwner) { response ->
            viewModel.getUserReview(fuelStationId!!)

            val text = if (response.isSuccessful) resources.getString(R.string.published)
                       else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initEditedReviewObserver() {
        viewModel.updateUserReview.observe(viewLifecycleOwner) { response ->
            viewModel.getUserReview(fuelStationId!!)

            val text = if (response.isSuccessful) resources.getString(R.string.edited_review)
                       else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initUserReview() {
        viewModel.getUserReview(fuelStationId!!)
        viewModel.userReview.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful && response.body() != null) {
                addUserReviewToReviewSection(response.body()!!)
                hideAddReviewButton()
            }
        }
    }

    private fun addUserReviewToReviewSection(review: Review) {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()
        val parent = binding.llcUserReviewContainer
        parent.removeAllViews()

        val reviewFragment = FuelStationReviewFragment(review)
        fragmentTransaction.add(parent.id, reviewFragment)

        fragmentTransaction.commitNow()
    }

    private fun initDeleteUserReviewObserver() {
        viewModel.deleteUserReview.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                removeUserReviewFromReviewSection()
                showAddReviewButton()
            }

            val text = if (response.isSuccessful) resources.getString(R.string.deleted)
                       else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initDeleteDiffUserReviewObserver() {
        viewModel.deleteDiffUserReview.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                refreshReviews()
                showReviewSectionProgressBar()
            }

            val text = if (response.isSuccessful) resources.getString(R.string.deleted)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun refreshReviews() {
        val parent = binding.llcReviewsContainer
        parent.removeAllViews()

        viewModel.getFirstPageOfFuelStationReviews(fuelStationId!!)
    }

    private fun initUserFavouriteObserver() {
        viewModel.getUserFavourite(fuelStationId!!)
        viewModel.userFavourite.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                prepareFavouriteButtonToRemoving()
            } else {
                prepareFavouriteButtonStateToAdding()
            }
        }
    }

    private fun initAddToFavouriteObserver() {
        viewModel.addToFavourite.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                prepareFavouriteButtonToRemoving()
            }

            val text = if (response.isSuccessful) resources.getString(R.string.added_to_favourite)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initRemoveFavouriteObserver() {
        viewModel.deleteFavourite.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                prepareFavouriteButtonStateToAdding()
            }

            val text = if (response.isSuccessful) resources.getString(R.string.removed_from_favourite)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initNewFuelPriceObserver() {
        viewModel.createNewFuelPrices.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                viewModel.getFuelStationDetails(viewModel.getFuelStationId()!!)
            }

            val text = if (response.isSuccessful) resources.getString(R.string.published)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initReportReviewObserver() {
        viewModel.reportReview.observe(viewLifecycleOwner) { response ->
            val text = if (response.isSuccessful) getString(R.string.reported)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun removeUserReviewFromReviewSection() {
        val parent = binding.llcUserReviewContainer
        parent.removeAllViews()
    }

    private fun initAddReviewButton() {
        val button = binding.mbRateFuelStation
        button.setOnClickListener {
            val reviewEditorFragment = FuelStationReviewEditorFragment(null, false)
            reviewEditorFragment.show(requireFragmentManager(), FuelStationReviewEditorFragment.TAG)
        }
    }

    private fun prepareFavouriteButtonToRemoving() {
        val button = binding.acibAddToFavourite
        button.isClickable = true
        button.setOnClickListener {
            button.isClickable = false
            viewModel.removeFuelStationFromFavourite(fuelStationId!!)
        }
        button.setImageResource(R.drawable.ic_baseline_star_24)
    }

    private fun prepareFavouriteButtonStateToAdding() {
        val button = binding.acibAddToFavourite
        button.isClickable = true
        button.setOnClickListener {
            button.isClickable = false
            viewModel.addFuelStationToFavourite(fuelStationId!!)
        }
        button.setImageResource(R.drawable.ic_baseline_star_border_24)
    }

    private fun showLayout() {
        binding.llMainFuelDetailsLayout.visibility = View.VISIBLE
        binding.fuelStationDetailsLoadingSpinner.visibility = View.GONE
    }

    private fun hideLayout() {
        binding.llMainFuelDetailsLayout.visibility = View.GONE
        binding.fuelStationDetailsLoadingSpinner.visibility = View.VISIBLE
    }

    private fun showAddReviewButton() {
        binding.mbRateFuelStation.visibility = View.VISIBLE
    }

    private fun hideAddReviewButton() {
        binding.mbRateFuelStation.visibility = View.GONE
    }

    private fun showReviewSectionProgressBar() {
        binding.pbReviewsLoad.visibility = View.VISIBLE
    }

    private fun hideReviewSectionProgressBar() {
        binding.pbReviewsLoad.visibility = View.GONE
    }

    override fun onDestroyView() {
        super.onDestroyView()
        viewModel.notifyAboutChanges()
        viewModel.clear()
        hideLayout()
    }

    companion object {
        const val TAG = "FuelStationDetails"
    }
}