package com.example.fuel.ui.fragment.favorites

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.ViewGroup
import android.widget.PopupMenu
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFavoriteFuelStationCardBinding
import com.example.fuel.model.favourite.UserFavouriteDetails
import com.example.fuel.ui.fragment.fuelstation.FuelStationDetailsFragment
import com.example.fuel.utils.calculateDistance
import com.example.fuel.utils.converters.UnitConverter
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FavouritesFuelStationsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.dialog.MaterialAlertDialogBuilder


class FavoriteFuelStationCardFragment(private val favourite: UserFavouriteDetails) : Fragment() {
    private lateinit var viewModel: FavouritesFuelStationsViewModel
    private lateinit var binding: FragmentFavoriteFuelStationCardBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FavouritesFuelStationsViewModel::class.java]
        binding = FragmentFavoriteFuelStationCardBinding.inflate(inflater, container, false)

        initWithData()
        initPopupMenu()
        initOnClickListener()

        return binding.root
    }

    private fun initWithData() {
        binding.tvFuelStationName.text = favourite.stationChain
        binding.tvAddress.text = favourite.address()
        setDistance(favourite.latitude, favourite.longitude)
    }

    private fun setDistance(latitude: Double, longitude: Double) {
        val textView = binding.tvDistance

        if (!isGpsEnabled(requireContext()) || getUserLocation(requireContext()) == null) {
            textView.visibility = View.GONE
            return
        }

        val location = getUserLocation(requireContext()) ?: return
        val distance = calculateDistance(location.latitude, location.longitude, latitude, longitude)

        textView.text =  resources.getString(R.string.from_you, UnitConverter.fromMetersToTarget(distance.toDouble()))
        textView.visibility = View.VISIBLE
    }

    private fun initPopupMenu() {
        val actionButton = binding.acibFavouriteActionButton

        actionButton.setOnClickListener {
            val popupMenu = PopupMenu(requireActivity(), actionButton)
            initPopupMenuWithActions(popupMenu)

            popupMenu.show()
        }
    }

    private fun initOnClickListener() {
        binding.clMainContainer.setOnClickListener {
            showFuelStationDetails()
        }
    }

    private fun initPopupMenuWithActions(popupMenu: PopupMenu) {
        val detailsItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 0, getString(R.string.details))
        val showOnMapItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, getString(R.string.show_on_map))
        val removeFromFav = popupMenu.menu.add(Menu.NONE, Menu.NONE, 2, getString(R.string.delete))

        detailsItem.setOnMenuItemClickListener {
            showFuelStationDetails()
            true
        }

        showOnMapItem.setOnMenuItemClickListener {
            showOnMap()
            true
        }

        removeFromFav.setOnMenuItemClickListener {
            removeFromFavourite()
            true
        }
    }

    private fun showFuelStationDetails() {
        val bundle = Bundle()
        bundle.putLong("fuelStationId", favourite.fuelStationId)

        val fuelStationDetails = FuelStationDetailsFragment()
        fuelStationDetails.arguments = bundle
        fuelStationDetails.show(requireFragmentManager(), FuelStationDetailsFragment.TAG)
    }

    private fun showOnMap() {
        val bundle = Bundle()
        bundle.putDouble("lat", favourite.latitude)
        bundle.putDouble("lon", favourite.longitude)

        Navigation.findNavController(binding.root).navigate(R.id.mapFragment, bundle)
    }

    private fun removeFromFavourite() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setMessage(getString(R.string.favourite_ask_if_delete))
            .setPositiveButton(resources.getString(R.string.yes)) { _, _ ->
                viewModel.removeFuelStationFromFavourite(favourite.fuelStationId)
            }
            .setNegativeButton(resources.getString(R.string.no), null)
            .show()
    }
}