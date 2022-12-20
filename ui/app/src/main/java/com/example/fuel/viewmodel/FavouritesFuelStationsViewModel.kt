package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.UserLocation
import com.example.fuel.model.favourite.UserFavouriteDetails
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.repository.FavouriteRepository
import com.example.fuel.viewmodel.mediator.FavouriteViewModelMediator
import kotlinx.coroutines.launch
import retrofit2.Response

class FavouritesFuelStationsViewModel(
    private val favouriteRepository: FavouriteRepository): ViewModel() {

    private val pageSize = 10
    private val sortProperty = "FuelStationId"
    private val sortDirection = "ASC"

    var favourites: MutableLiveData<Response<Page<UserFavouriteDetails>>> = MutableLiveData()
    var deleteFavourite: MutableLiveData<Response<Void>> = MutableLiveData()

    var userLocation: UserLocation? = null

    fun getFirstPageOfFavourites() {
        viewModelScope.launch {
            val pageRequest = PageRequest(1, pageSize, sortProperty, sortDirection)
            favourites.value = favouriteRepository.getAllUserFavourites(pageRequest)
        }
    }

    fun getNextPageOfFavourites() {
        viewModelScope.launch {
            val nextPage = (if (favourites.value != null) favourites.value?.body()?.nextPage else 1) ?: return@launch

            val pageRequest = PageRequest(nextPage, pageSize, sortProperty, sortDirection)
            favourites.value = favouriteRepository.getAllUserFavourites(pageRequest)
        }
    }

    fun removeFuelStationFromFavourite(fuelStationId: Long) {
        viewModelScope.launch {
            deleteFavourite.value = favouriteRepository.deleteFromFavourite(fuelStationId)
            FavouriteViewModelMediator.favouriteChanged()
        }
    }

    fun hasMoreFavourites(): Boolean = favourites.value?.body()?.nextPage != null

    fun hasAnyFavourites(): Boolean = favourites.value?.body()?.totalElements != null
            && favourites.value!!.body()!!.totalElements > 0

    fun setUserLocation(lat: Double, lon: Double) {
        userLocation = UserLocation(lat, lon)
    }

    fun isFirstPage(): Boolean = favourites.value?.body()?.pageNumber == 1

    fun init() {
        FavouriteViewModelMediator.subscribe(this)
    }

    fun clear() {
        favourites = MutableLiveData()
        deleteFavourite = MutableLiveData()

        FavouriteViewModelMediator.unsubscribe()
    }
}