package com.example.fuel.ui.fragment.map

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.example.fuel.R
import com.example.fuel.databinding.FragmentMapFilterBinding
import com.example.fuel.mock.getFuelStationServices
import com.example.fuel.mock.getFuelTypes
import com.example.fuel.mock.getStationChains
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelType
import com.example.fuel.model.StationChain
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.ui.common.toggleChipAppearance
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import com.google.android.material.slider.RangeSlider

class MapFilter : Fragment(R.layout.fragment_map_filter) {
    private lateinit var binding: FragmentMapFilterBinding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentMapFilterBinding.inflate(inflater, container, false)

        initFuelTypeChips()
        initStationChainChips()
        initFuelStationServiceChips()
        initFuelPriceRangeSlider()

        return binding.root
    }

    private fun initFuelTypeChips() {
        val fuelTypeChipGroup: ChipGroup = binding.cgMapFuelTypes
        getFuelTypes().forEach { fuelType ->
            fuelTypeChipGroup.addView(createFuelTypeChip(fuelType))
        }
    }

    private fun createFuelTypeChip(fuelType: FuelType): Chip {
        val chip = createGenericChip()
        chip.text = fuelType.name
        return chip
    }

    private fun initStationChainChips() {
        val stationChainChipGroup: ChipGroup = binding.cgMapStationChains
        getStationChains().forEach { stationChain ->
            stationChainChipGroup.addView(createStationChainChip(stationChain))
        }
    }

    private fun createStationChainChip(stationChain: StationChain): Chip {
        val chip = createGenericChip()
        chip.text = stationChain.name
        return chip
    }

    private fun initFuelStationServiceChips() {
        val fuelStationServiceChipGroup: ChipGroup = binding.cgMapFuelStationServices
        getFuelStationServices().forEach { fuelStationService ->
            fuelStationServiceChipGroup.addView(createFuelStationServiceChip(fuelStationService))
        }
    }

    private fun createFuelStationServiceChip(fuelStationService: FuelStationService): Chip {
        val chip = createGenericChip()
        chip.text = fuelStationService.name
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
        slider.setValues(0F, 15F)
    }
}