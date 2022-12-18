package com.example.fuel.ui.fragment.fuelstationlist

import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.Menu
import android.view.MenuInflater
import android.view.MenuItem
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AppCompatActivity
import androidx.core.widget.NestedScrollView
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationListBinding
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FuelStationListViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.dialog.MaterialAlertDialogBuilder

class FuelStationListFragment : Fragment(R.layout.fragment_map) {
    private lateinit var binding: FragmentFuelStationListBinding
    private lateinit var viewModel: FuelStationListViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        setHasOptionsMenu(true)

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationListViewModel::class.java]
        binding = FragmentFuelStationListBinding.inflate(inflater, container, false)

//        binding.btnGoToMap.setOnClickListener {
//            Navigation.findNavController(binding.root).navigate(R.id.mapFragment)
//        }

        initUserLocation()
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

    override fun onCreateOptionsMenu(menu: Menu, inflater: MenuInflater) {
        inflater.inflate(R.menu.list_menu, menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        if (item.itemId == R.id.list_sort) {
            openSortDialog()
        }

        if (item.itemId == R.id.list_filter) {

        }

        return super.onOptionsItemSelected(item)
    }

    private fun openSortDialog() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setTitle(getString(R.string.choose_sort))
            .setSingleChoiceItems(viewModel.sortOptions(), viewModel.currentSort()) { _, which ->
                viewModel.choiceSort(which)
            }
            .setPositiveButton(getString(R.string.ok)) { dialog, which ->
                refreshFuelStations()
                dialog.dismiss()
            }
            .setNegativeButton(getString(R.string.cancel)) { dialog, which ->
                viewModel.cancelSort()
                dialog.dismiss()
            }
            .show()
    }

    private fun refreshFuelStations() {
        val parent = binding.llcFuelStationsContainer
        parent.removeAllViews()

        showFuelStationProgressBar()

        viewModel.getFirstPageOfFuelStations()
    }

    private fun initUserLocation() {
        if (isGpsEnabled(requireContext())) {
            val location = getUserLocation(requireContext()) ?: return
            viewModel.setUserLocation(location.latitude, location.longitude)
        }
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)

        val appActivity = (activity as AppCompatActivity)
        if (!appActivity.supportActionBar?.isShowing!!) {
            appActivity.supportActionBar?.show()
        }
    }

    override fun onDestroy() {
        super.onDestroy()

        viewModel.clear()
    }
}