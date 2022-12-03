package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelPriceCardBinding
import com.example.fuel.model.FuelTypeWithPrice
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory


class FuelPriceCardFragment(
    private val fuelTypeWithPrice: FuelTypeWithPrice) : Fragment(R.layout.fragment_fuel_price_card) {

    private lateinit var binding: FragmentFuelPriceCardBinding
    private lateinit var viewModel: FuelStationDetailsViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelPriceCardBinding.inflate(inflater, container, false)

        initWithData()

        return binding.root
    }

    private fun initWithData() {
        binding.tvFuelTypeName.text = fuelTypeWithPrice.name
        binding.tvFuelPrice.text = viewModel.parsePrice(fuelTypeWithPrice.fuelPrice, resources)
        binding.tvFuelPriceLastUpdateDate.text = viewModel.parseFuelPriceCreatedAt(fuelTypeWithPrice.fuelPrice, resources)
    }
}