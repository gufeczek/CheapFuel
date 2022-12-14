package com.example.fuel.ui.fragment.fuelstation

import android.graphics.Color
import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelTypePriceEditorBinding
import com.example.fuel.model.FuelTypeWithPrice
import com.example.fuel.model.price.NewFuelPrice
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory


class FuelTypePriceEditorFragment(
    private val fuelTypeWithPrice: FuelTypeWithPrice,
    private val newFuelPrice: NewFuelPrice) : Fragment(R.layout.fragment_fuel_type_price_editor) {

    private lateinit var binding: FragmentFuelTypePriceEditorBinding
    private lateinit var viewModel: FuelStationDetailsViewModel

    private var state: Boolean = true

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelTypePriceEditorBinding.inflate(inflater, container, false)

        initWithData()

        return binding.root;
    }

    private fun initWithData() {
        binding.tvFuelTypeName.text = fuelTypeWithPrice.name
        binding.tvFuelPriceLastUpdateDate.text = viewModel.parseFuelPriceCreatedAt(fuelTypeWithPrice.fuelPrice, resources)
        binding.tietPriceContent.setText(newFuelPrice.price.toString())

        initAvailableButton()
        initPriceListener()

        if (newFuelPrice.available) initFuelAvailableState()
        else initFuelNotAvailableState()
    }

    private fun initAvailableButton() {
        val availableButton = binding.acibFuelAvailableButton
        availableButton.setImageResource(R.drawable.ic_fuel_green_32)

        availableButton.setOnClickListener {
            state = !state
            newFuelPrice.available = state

            if (state) {
                initFuelAvailableState()
            } else {
                initFuelNotAvailableState()
            }
        }
    }

    private fun initPriceListener() {
        binding.tietPriceContent.addTextChangedListener(object: TextWatcher {
            override fun beforeTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) { }

            override fun onTextChanged(p0: CharSequence?, p1: Int, p2: Int, p3: Int) { }

            override fun afterTextChanged(p0: Editable?) {
                val price = if (p0.isNullOrBlank()) null else p0.toString().toDoubleOrNull()

                if (price == null) {
                    newFuelPrice.price = 0.0
                } else {
                    newFuelPrice.price = price
                }
            }
        })
    }

    private fun initFuelAvailableState() {
        binding.acibFuelAvailableButton.setImageResource(R.drawable.ic_fuel_green_32)
        binding.tvFuelTypeName.setTextColor(resources.getColor(R.color.black, activity?.theme))
        binding.tvCurrency.setTextColor(resources.getColor(R.color.black, activity?.theme))
        binding.tilPriceContent.isEnabled = true
    }

    private fun initFuelNotAvailableState() {
        binding.acibFuelAvailableButton.setImageResource(R.drawable.ic_fuel_black_32)
        binding.tvFuelTypeName.setTextColor(resources.getColor(R.color.dark_gray, activity?.theme))
        binding.tvCurrency.setTextColor(resources.getColor(R.color.dark_gray, activity?.theme))
        binding.tilPriceContent.isEnabled = false
        binding.tilPriceContent.boxBackgroundColor = Color.TRANSPARENT
    }

}