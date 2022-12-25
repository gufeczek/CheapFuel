package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.databinding.FragmentFuelPriceEditorBinding
import com.example.fuel.model.price.NewFuelPrice
import com.example.fuel.ui.fragment.common.FullHeightBottomSheetDialogFragment
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory


class FuelPriceEditorFragment : FullHeightBottomSheetDialogFragment() {
    private lateinit var viewModel: FuelStationDetailsViewModel
    private lateinit var binding: FragmentFuelPriceEditorBinding

    private var newFuelPrices: Array<NewFuelPrice> = emptyArray()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelPriceEditorBinding.inflate(inflater, container, false)

        addFuelTypes()
        initPublishButton()
        initKeyboardHiding()

        return binding.root
    }

    private fun addFuelTypes() {
        val fuelTypes = viewModel.getFuelTypes() ?: return

        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        val parent = binding.llcFuelTypeContainer

        for (fuelType in fuelTypes) {
            val newFuelPrice = NewFuelPrice(
                fuelType.id,
                fuelType.fuelPrice?.price ?: 0.0,
                fuelType.fuelPrice?.available ?: false)
            newFuelPrices = newFuelPrices.plus(newFuelPrice)

            val fuelTypePriceEditorFragment = FuelTypePriceEditorFragment(fuelType, newFuelPrice)
            fragmentTransaction.add(parent.id, fuelTypePriceEditorFragment)
        }

        fragmentTransaction.commitNow()
    }

    private fun initPublishButton() {
        binding.mbPublishPrices.setOnClickListener {
            val userLocation = if (isGpsEnabled(requireContext())) getUserLocation(requireContext())
                               else null
            viewModel.createNewFuelPrices(newFuelPrices, userLocation)
            dismiss()
        }
    }

    private fun initKeyboardHiding() {
        val main = binding.clFuelPriceEditorContainer
        main.setOnClickListener { view -> view.hideKeyboard() }
    }

    companion object {
        const val TAG = "FuelPriceEditor"
    }
}