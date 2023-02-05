package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelType
import com.example.fuel.model.StationChain
import com.example.fuel.model.fuelstation.NewFuelStation
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.FuelStationServiceRepository
import com.example.fuel.repository.FuelTypeRepository
import com.example.fuel.repository.StationChainRepository
import kotlinx.coroutines.launch
import retrofit2.Response

class NewFuelStationViewModel(
    private val stationChainRepository: StationChainRepository,
    private val fuelStationRepository: FuelStationRepository,
    private val fuelTypeRepository: FuelTypeRepository,
    private val serviceRepository: FuelStationServiceRepository): ViewModel() {

    var stationChains: MutableLiveData<Response<Page<StationChain>>> = MutableLiveData()
    var fuelTypes: MutableLiveData<Response<Page<FuelType>>> = MutableLiveData()
    var services: MutableLiveData<Response<Page<FuelStationService>>> = MutableLiveData()
    var newFuelStation: MutableLiveData<Response<FuelStationDetails>> = MutableLiveData()

    var stationChainId: Long? = null
    var fuelTypesIds: MutableList<Long> = mutableListOf()
    var servicesIds: MutableList<Long> = mutableListOf()

    fun getStationChains() {
        viewModelScope.launch {
            val pageResponse = PageRequest(1, 100, "Id", "ASC")
            stationChains.value = stationChainRepository.getStationChains(pageResponse)
        }
    }

    fun getFuelTypes() {
        viewModelScope.launch {
            val pageResponse = PageRequest(1, 100, "Id", "ASC")
            fuelTypes.value = fuelTypeRepository.getFuelTypes(pageResponse)
        }
    }

    fun getFuelStationServices() {
        viewModelScope.launch {
            val pageResponse = PageRequest(1, 100, "Id", "ASC")
            services.value = serviceRepository.getFuelStationServices(pageResponse)
        }
    }

    fun createFuelStation(fuelStation: NewFuelStation) {
        viewModelScope.launch {
            newFuelStation.value = fuelStationRepository.createFuelStation(fuelStation)
        }
    }

    fun onStationChainSelected(stationChainId: Long) {
        this.stationChainId = stationChainId
    }

    fun onFuelTypeSelected(fuelTypeId: Long) {
        addOrRemove(fuelTypesIds, fuelTypeId)
    }

    fun onServiceSelected(serviceId: Long) {
        addOrRemove(servicesIds, serviceId)
    }

    private fun addOrRemove(list: MutableList<Long>, value: Long) {
        if (list.contains(value)) {
            list.removeAll { it == value }
        } else {
            list.add(value)
        }
    }

    fun isStationChainSelected(stationChainId: Long): Boolean =
        this.stationChainId == stationChainId

    fun isFuelTypeSelected(fuelTypeId: Long): Boolean =
        this.fuelTypesIds.contains(fuelTypeId)

    fun isServiceSelected(serviceId: Long): Boolean =
        this.servicesIds.contains(serviceId)

    fun getStationChainId(): Long = stationChainId!!

    fun getFuelTypesIds(): Array<Long> =
        this.fuelTypesIds.toTypedArray()

    fun getServicesIds(): Array<Long> =
        this.servicesIds.toTypedArray()
}