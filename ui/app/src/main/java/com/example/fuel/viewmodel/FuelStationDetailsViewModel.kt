package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.repository.FuelStationRepository
import kotlinx.coroutines.launch
import retrofit2.Response

class FuelStationDetailsViewModel(
    private val fuelStationRepository: FuelStationRepository): ViewModel() {

    var fuelStationDetails: MutableLiveData<Response<FuelStationDetails>> = MutableLiveData()

    fun getFuelStationDetails(fuelStationId: Long) {
        viewModelScope.launch {
            fuelStationDetails.value = fuelStationRepository.getFuelStationDetails(fuelStationId)
        }
    }

    fun clear() {
        fuelStationDetails = MutableLiveData()
    }
}