package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
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
    private lateinit var fuelPriceCardView: View

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        binding = FragmentFuelPriceCardBinding.inflate(inflater, container, false)
        fuelPriceCardView = inflater.inflate(R.layout.fragment_fuel_price_card, container, false)

        initWithData()

        return fuelPriceCardView
    }

    private fun initWithData() {
        val fuelTypeNameTextView = fuelPriceCardView.findViewById<TextView>(R.id.tv_fuelTypeName)
        fuelTypeNameTextView.text = fuelTypeWithPrice.name

        val fuelPriceTextView = fuelPriceCardView.findViewById<TextView>(R.id.tv_fuelPrice)
        fuelPriceTextView.text = viewModel.parsePrice(fuelTypeWithPrice.fuelPrice, resources)

        val timePeriodTextView = fuelPriceCardView.findViewById<TextView>(R.id.tv_fuelPriceLastUpdateDate)
        timePeriodTextView.text = viewModel.parseFuelPriceCreatedAt(fuelTypeWithPrice.fuelPrice, resources)
    }
}