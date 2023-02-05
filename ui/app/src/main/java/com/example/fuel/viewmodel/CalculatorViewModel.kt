package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.FuelType
import com.example.fuel.model.calculator.MostEconomicalFuelStationRequest
import com.example.fuel.model.calculator.MostEconomicalFuelStationResponse
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.FuelTypeRepository
import com.example.fuel.utils.validation.ValidatorNumber
import kotlinx.coroutines.launch
import retrofit2.Response

class CalculatorViewModel(
    private val fuelStationRepository: FuelStationRepository,
    private val fuelTypeRepository: FuelTypeRepository): ViewModel() {

    var fuelStation: MutableLiveData<Response<MostEconomicalFuelStationResponse>> = MutableLiveData()
    var fuelTypes: MutableLiveData<Response<Page<FuelType>>> = MutableLiveData()

    var data = MostEconomicalFuelStationRequest()
    var isValid = false

    fun getFuelTypes() {
        viewModelScope.launch {
            val pageRequest = PageRequest(1, 100, "Name", "ASC")
            fuelTypes.value = fuelTypeRepository.getFuelTypes(pageRequest)
        }
    }

    fun calculateEconomicalFuelStation() {
        viewModelScope.launch {
            fuelStation.value = fuelStationRepository.getMostEconomicalFuelStation(data)
        }
    }

    fun onFuelTypeSelected(fuelTypeId: Long) {
        data.fuelTypeId = fuelTypeId
    }

    fun onLatitudeChange(latitude: Double?) {
        data.userLatitude = latitude
    }

    fun onLongitudeChange(longitude: Double?) {
        data.userLongitude = longitude
    }

    fun onFuelConsumptionChange(consumption: Double?) {
        data.fuelConsumption = consumption
    }

    fun onAmountChange(amount: Double?) {
        data.fuelAmountToBuy = amount
    }

    fun isFuelTypeSelected(fuelTypeId: Long): Boolean =
        data.fuelTypeId == fuelTypeId

    fun validate() {
        isValid = isLatitudeValid() &&
                isLongitudeValid() &&
                isFuelConsumptionValid() &&
                isAmountToBuyValid() &&
                isFuelTypeValid()
    }

    fun isLatitudeValid(): Boolean =
        ValidatorNumber(data.userLatitude, -90.0, 90.0).validate()

    fun isLongitudeValid(): Boolean =
        ValidatorNumber(data.userLongitude, -180.0, 180.0).validate()

    fun isFuelConsumptionValid(): Boolean =
        ValidatorNumber(data.fuelConsumption, 0.0, null).validate()

    fun isAmountToBuyValid(): Boolean =
        ValidatorNumber(data.fuelAmountToBuy, 0.0, null).validate()

    fun isFuelTypeValid(): Boolean = data.fuelTypeId != null

    fun clear() {
        fuelStation = MutableLiveData()
        fuelTypes = MutableLiveData()
        data = MostEconomicalFuelStationRequest()
        isValid = false
    }
}