package com.example.fuel.ui.fragment.fuelstationlist

import android.content.Context
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AppCompatActivity
import androidx.core.widget.NestedScrollView
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationListBinding
import com.example.fuel.viewmodel.FuelStationListViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class FuelStationListFragment : Fragment(R.layout.fragment_map) {
    private lateinit var binding: FragmentFuelStationListBinding
    private lateinit var viewModel: FuelStationListViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationListViewModel::class.java]
        binding = FragmentFuelStationListBinding.inflate(inflater, container, false)

//        binding.btnGoToMap.setOnClickListener {
//            Navigation.findNavController(binding.root).navigate(R.id.mapFragment)
//        }

        initFuelStationsSection()
        initFuelStationObserver()

        return binding.root
    }

    private fun initFuelStationsSection() {
        viewModel.getFirstPageOfFuelStations()

        binding.nsvFuelStationsList
            .setOnScrollChangeListener { v, _, scrollY, _, oldScrollY ->
                val nsv = v as NestedScrollView

                if (oldScrollY < scrollY
                    && scrollY == (nsv.getChildAt(0).measuredHeight - nsv.measuredHeight)
                    && viewModel.hasMoreFuelStations()) {

                    loadFuelStations()
                }
            }
    }

    private fun loadFuelStations() {
        viewModel.getNextPageOfFuelStations()
    }

    private fun initFuelStationObserver() {
        viewModel.fuelStations.observe(viewLifecycleOwner) { response ->
            val fragmentManager = childFragmentManager
            val fragmentTransaction = fragmentManager.beginTransaction()
            val parent = binding.llcFuelStationsContainer

            val page = response.body()
            Log.d("Test", page!!.nextPage.toString())

            for (fuelStation in page?.data!!) {
                val fuelStationFragment = FuelStationListCardFragment(fuelStation)
                fragmentTransaction.add(parent.id, fuelStationFragment)
            }
            fragmentTransaction.commitNow()

            if (!viewModel.hasMoreFuelStations()) hideFuelStationProgressBar()
        }
    }

    private fun showFuelStationProgressBar() {
        binding.pbFuelStationsLoad.visibility = View.VISIBLE
    }

    private fun hideFuelStationProgressBar() {
        binding.pbFuelStationsLoad.visibility = View.GONE
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)

        (activity as AppCompatActivity).supportActionBar?.show()
    }

    override fun onDestroy() {
        super.onDestroy()

        viewModel.clear()
    }
}