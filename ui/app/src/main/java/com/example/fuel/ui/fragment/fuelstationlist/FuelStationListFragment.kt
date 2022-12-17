package com.example.fuel.ui.fragment.fuelstationlist

import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationListBinding

class FuelStationListFragment : Fragment(R.layout.fragment_map) {
    private lateinit var binding: FragmentFuelStationListBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        binding = FragmentFuelStationListBinding.inflate(inflater, container, false)

        binding.btnGoToMap.setOnClickListener {
            Navigation.findNavController(binding.root).navigate(R.id.mapFragment)
        }

        return binding.root
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)

        (activity as AppCompatActivity).supportActionBar?.show()
    }
}