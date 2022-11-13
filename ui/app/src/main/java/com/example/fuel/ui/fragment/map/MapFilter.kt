package com.example.fuel.ui.fragment.map

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.lifecycleScope
import com.example.fuel.R
import com.example.fuel.databinding.FragmentMapFilterBinding
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelType
import com.example.fuel.model.StationChain
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.ui.common.toggleChipAppearance
import com.example.fuel.utils.DataLoadedIndicator
import com.example.fuel.viewmodel.FuelStationMapViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import com.google.android.material.slider.RangeSlider
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch


class MapFilter : Fragment(R.layout.fragment_map_filter) {
    private lateinit var binding: FragmentMapFilterBinding
    private lateinit var viewModel: FuelStationMapViewModel
    private lateinit var dataLoadedIndicator: DataLoadedIndicator

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationMapViewModel::class.java]
        binding = FragmentMapFilterBinding.inflate(inflater, container, false)
        dataLoadedIndicator = DataLoadedIndicator(3)
        lifecycleScope.launch { collectDataLoaded() }

        viewModel.initFilter(1) // Just in case
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

        slider.addOnSliderTouchListener(object: RangeSlider.OnSliderTouchListener {
            override fun onStartTrackingTouch(slider: RangeSlider) {}

            override fun onStopTrackingTouch(slider: RangeSlider) {
                val values = slider.values
                viewModel.onPriceRangeChanged(values[0], values[1])
            }
        })
    }
}