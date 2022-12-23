package com.example.fuel.ui.fragment.fuelstationlist

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.lifecycleScope
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationListFilterBinding
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelType
import com.example.fuel.model.StationChain
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.ui.common.toggleChipAppearance
import com.example.fuel.utils.DataLoadedIndicator
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FuelStationListViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.checkbox.MaterialCheckBox
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import com.google.android.material.slider.RangeSlider
import com.google.android.material.slider.Slider
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

class FuelStationListFilterFragment : Fragment(R.layout.fragment_fuel_station_list_filter) {
    private lateinit var binding: FragmentFuelStationListFilterBinding
    private lateinit var viewModel: FuelStationListViewModel
    private lateinit var dataLoadedIndicator: DataLoadedIndicator

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationListViewModel::class.java]
        binding = FragmentFuelStationListFilterBinding.inflate(inflater, container, false)
        dataLoadedIndicator = DataLoadedIndicator(3)
        lifecycleScope.launch { collectDataLoaded() }

        viewModel.initFilter(1)
        viewModel.initDataForFilter()

        initFragmentWithStartingData()

        return binding.root
    }

    private suspend fun collectDataLoaded() {
        dataLoadedIndicator.isAllDataLoaded().collectLatest {
            binding.loadingSpinner.visibility = View.GONE
            binding.svFilterContainer.visibility = View.VISIBLE
        }
    }

    private fun initFragmentWithStartingData() {
        initFuelTypeChips()
        initStationChainChips()
        initFuelStationServiceChips()
        initFuelPriceRangeSlider()
        initMaxDistanceSection()
    }

    private fun initFuelTypeChips() {
        viewModel.fuelTypes.observe(viewLifecycleOwner) { response ->
            val fuelTypeChipGroup: ChipGroup = binding.cgMapFuelTypes
            fuelTypeChipGroup.removeAllViews()

            response.body()?.data?.forEach { fuelType ->
                fuelTypeChipGroup.addView(createFuelTypeChip(fuelType))
            }

            lifecycleScope.launch { dataLoadedIndicator.inc() }
        }
    }

    private fun createFuelTypeChip(fuelType: FuelType): Chip {
        val chip = createGenericChip()
        chip.text = fuelType.name
        chip.setOnClickListener { viewModel.onFuelTypeSelected(fuelType.id) }
        chip.isChecked = viewModel.isFuelTypeSelected(fuelType.id)
        return chip
    }

    private fun initStationChainChips() {
        viewModel.stationChains.observe(viewLifecycleOwner) { response ->
            val stationChainChipGroup: ChipGroup = binding.cgMapStationChains
            stationChainChipGroup.removeAllViews()

            response.body()?.data?.forEach { stationChain ->
                stationChainChipGroup.addView(createStationChainChip(stationChain))
            }

            lifecycleScope.launch { dataLoadedIndicator.inc() }
        }
    }

    private fun createStationChainChip(stationChain: StationChain): Chip {
        val chip = createGenericChip()
        chip.text = stationChain.name
        chip.setOnClickListener { viewModel.onStationChainSelected(stationChain.id) }
        chip.isChecked = viewModel.isStationChainSelected(stationChain.id)
        return chip
    }

    private fun initFuelStationServiceChips() {
        viewModel.fuelStationServices.observe(viewLifecycleOwner) { response ->
            val fuelStationServiceChipGroup: ChipGroup = binding.cgMapFuelStationServices
            fuelStationServiceChipGroup.removeAllViews()

            response.body()?.data?.forEach { fuelStationService ->
                fuelStationServiceChipGroup.addView(createFuelStationServiceChip(fuelStationService))
            }

            lifecycleScope.launch { dataLoadedIndicator.inc() }
        }
    }

    private fun createFuelStationServiceChip(fuelStationService: FuelStationService): Chip {
        val chip = createGenericChip()
        chip.text = fuelStationService.name
        chip.setOnClickListener { viewModel.onFuelStationServiceSelected(fuelStationService.id) }
        chip.isChecked = viewModel.isFuelStationServiceSelected(fuelStationService.id)
        return chip
    }

    private fun createGenericChip(): Chip {
        val chip = LayoutInflater.from(requireContext()).inflate(R.layout.filter_chip, null, false) as Chip
        initChipAppearance(chip, requireContext())
        chip.setOnCheckedChangeListener { button, _ -> toggleChipAppearance(button as Chip, requireContext()) }
        return chip
    }

    private fun initFuelPriceRangeSlider() {
        val slider: RangeSlider = binding.rgFuelPriceRange
        slider.setValues(viewModel.currentMinPrice(), viewModel.currentMaxPrice())

        slider.addOnSliderTouchListener(object : RangeSlider.OnSliderTouchListener {
            override fun onStartTrackingTouch(slider: RangeSlider) {}

            override fun onStopTrackingTouch(slider: RangeSlider) {
                val values = slider.values
                viewModel.onPriceRangeChanged(values[0], values[1])
            }
        })
    }

    private fun initMaxDistanceSection() {
        if (!isGpsEnabled(requireContext())) {
            hideDistanceSection()
        } else {
            setUserLocation()
        }

        initMaxDistanceCheckbox()
        initMaxDistanceSlider()
    }

    private fun initMaxDistanceCheckbox() {
        val checkbox: MaterialCheckBox = binding.mcbMaxDistance
        checkbox.isChecked = viewModel.isDistanceSet()

        checkbox.setOnCheckedChangeListener { _, isChecked ->
            val slider = binding.rgDistanceRange
            slider.isEnabled = isChecked

            if (isChecked) {
                viewModel.onDistanceChange(slider.value)
            } else {
                viewModel.onDistanceChange(null)
            }
        }
    }

    private fun initMaxDistanceSlider() {
        val slider: Slider = binding.rgDistanceRange
        slider.value = viewModel.currentDistance()
        slider.isEnabled = viewModel.isDistanceSet()

        slider.addOnSliderTouchListener(object : Slider.OnSliderTouchListener {
            override fun onStartTrackingTouch(slider: Slider) { }

            override fun onStopTrackingTouch(slider: Slider) {
                viewModel.onDistanceChange(slider.value)
            }
        })
    }

    private fun hideDistanceSection() {
        binding.llcDistance.visibility = View.GONE
        viewModel.onDistanceChange(null)
    }

    private fun setUserLocation() {
        val location = getUserLocation(requireContext())

        if (location == null && !viewModel.isUserLocationSet()) {
            hideDistanceSection()
        } else {
            viewModel.setUserLocation(location!!.latitude, location.longitude)
        }
    }
}