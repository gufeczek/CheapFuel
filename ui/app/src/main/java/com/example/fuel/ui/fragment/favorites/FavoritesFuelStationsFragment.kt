package com.example.fuel.ui.fragment.favorites

import android.content.Context
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ScrollView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.widget.NestedScrollView
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFavoritesFuelStationsBinding
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FavouritesFuelStationsViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class FavoritesFuelStationsFragment : Fragment(R.layout.fragment_favorites_fuel_stations) {
    private lateinit var binding: FragmentFavoritesFuelStationsBinding
    private lateinit var viewModel: FavouritesFuelStationsViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FavouritesFuelStationsViewModel::class.java]
        viewModel.init()

        binding = FragmentFavoritesFuelStationsBinding.inflate(inflater, container, false)

        initUserLocation()
        initFavouritesSection()
        initFavouriteObserver()
        initRemoveFavouriteObserver()

        return binding.root
    }

    private fun initFavouritesSection() {
        viewModel.getFirstPageOfFavourites()

        binding.nsvFuelStationsFavorites
            .setOnScrollChangeListener { v, _, scrollY, _, oldScrollY ->
                val nsv = v as NestedScrollView

                if (oldScrollY < scrollY
                    && scrollY == (nsv.getChildAt(0).measuredHeight - nsv.measuredHeight)
                    && viewModel.hasMoreFavourites()) {

                    loadFavourites()
                }
            }
    }

    private fun loadFavourites() {
        viewModel.getNextPageOfFavourites()
    }

    private fun initFavouriteObserver() {
        viewModel.favourites.observe(viewLifecycleOwner) { response ->
            val fragmentManager = childFragmentManager
            val fragmentTransaction = fragmentManager.beginTransaction()
            val parent = binding.llcFavouriteContainer

            if (viewModel.isFirstPage()) {
                showFuelStationProgressBar()
                scrollToTop()
                parent.removeAllViews()
            }

            if (!viewModel.hasAnyFavourites()) {
                hideFuelStationProgressBar()
                showPlaceholder()
                return@observe
            } else {
                hidePlaceholder()
            }

            val page = response.body()

            for (favourite in page?.data!!) {
                val favouriteFragment = FavoriteFuelStationCardFragment(favourite)
                fragmentTransaction.add(parent.id, favouriteFragment)
            }
            fragmentTransaction.commitNow()

            if (!viewModel.hasMoreFavourites()) hideFuelStationProgressBar()
        }
    }

    private fun scrollToTop() {
        binding.nsvFuelStationsFavorites.fullScroll(ScrollView.FOCUS_UP)
    }

    private fun showFuelStationProgressBar() {
        binding.pbFavouriteLoad.visibility = View.VISIBLE
    }

    private fun hideFuelStationProgressBar() {
        binding.pbFavouriteLoad.visibility = View.GONE
    }

    private fun initRemoveFavouriteObserver() {
        viewModel.deleteFavourite.observe(viewLifecycleOwner) { response ->
            viewModel.getFirstPageOfFavourites()

            val text = if (response.isSuccessful) resources.getString(R.string.removed_from_favourite)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initUserLocation() {
        if (isGpsEnabled(requireContext())) {
            val location = getUserLocation(requireContext()) ?: return
            viewModel.setUserLocation(location.latitude, location.longitude)
        }
    }

    private fun showPlaceholder() {
        binding.clPlaceholder.visibility = View.VISIBLE
    }

    private fun hidePlaceholder() {
        binding.clPlaceholder.visibility = View.GONE
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)

        val appActivity = (activity as AppCompatActivity)
        if (!appActivity.supportActionBar?.isShowing!!) {
            appActivity.supportActionBar?.show()
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()

        viewModel.clear()
    }
}