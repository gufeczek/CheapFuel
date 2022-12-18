package com.example.fuel.ui.fragment.fuelstationlist

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationListCardBinding
import com.example.fuel.model.SimpleFuelStation
import com.example.fuel.utils.calculateDistance
import com.example.fuel.utils.converters.UnitConverter
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FuelStationListViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class FuelStationListCardFragment(private val fuelStation: SimpleFuelStation) : Fragment() {

    private lateinit var viewModel: FuelStationListViewModel
    private lateinit var binding: FragmentFuelStationListCardBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationListViewModel::class.java]
        binding = FragmentFuelStationListCardBinding.inflate(inflater, container, false)

        initWithData()

        return binding.root
    }

    private fun initWithData() {
        binding.tvFuelStationName.text = fuelStation.stationChainName
        binding.tvFuelPrice.text = viewModel.parsePrice(fuelStation.price, resources)
        binding.tvFuelPriceLastUpdateDate.text = viewModel.parseFuelPriceLastUpdate(fuelStation.lastFuelPriceUpdate, resources)
        setDistance(fuelStation.latitude, fuelStation.longitude)
    }

    private fun setDistance(latitude: Double, longitude: Double) {
        val textView = binding.tvDistanceBetweenUserAndStation

        if (!isGpsEnabled(requireContext())) {
            textView.visibility = View.GONE
            return
        }

        val location = getUserLocation(requireContext()) ?: return
        val distance = calculateDistance(location.latitude, location.longitude, latitude, longitude)

        textView.text =  resources.getString(R.string.from_you, UnitConverter.fromMetersToTarget(distance.toDouble()))
        textView.visibility = View.VISIBLE
    }
}