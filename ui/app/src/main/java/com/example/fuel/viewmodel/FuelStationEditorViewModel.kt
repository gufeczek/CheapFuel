package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelType
import com.example.fuel.model.fuelAtStation.AddFuelToStation
import com.example.fuel.model.fuelAtStation.FuelAtStation
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.model.serviceAtStation.AddServiceToStation
import com.example.fuel.model.serviceAtStation.ServiceAtStation
import com.example.fuel.repository.FuelAtStationRepository
import com.example.fuel.repository.FuelStationServiceRepository
import com.example.fuel.repository.FuelTypeRepository
import com.example.fuel.repository.ServiceAtStationRepository
import com.example.fuel.viewmodel.mediator.ListViewModelMediator
import com.example.fuel.viewmodel.mediator.MapViewModelMediator
import kotlinx.coroutines.launch
import retrofit2.Response

class FuelStationEditorViewModel(
    private val fuelAtStationRepository: FuelAtStationRepository,
    private val serviceAtStationRepository: ServiceAtStationRepository,
    private val fuelTypeRepository: FuelTypeRepository,
    private val fuelStationServiceRepository: FuelStationServiceRepository): ViewModel() {

    var fuelTypes: MutableLiveData<Response<Page<FuelType>>> = MutableLiveData()
    var fuelStationServices: MutableLiveData<Response<Page<FuelStationService>>> = MutableLiveData()

    var addFuelType: MutableLiveData<Response<FuelAtStation>> = MutableLiveData()
    var removeFuelType: MutableLiveData<Response<Void>> = MutableLiveData()
    var addService: MutableLiveData<Response<ServiceAtStation>> = MutableLiveData()
    var removeService: MutableLiveData<Response<Void>> = MutableLiveData()

    private var fuelStation: FuelStationDetails? = null
    private var isChange: Boolean = false

    fun getFuelTypes() {
        viewModelScope.launch {
            val pageRequest = PageRequest(1, 100, "Name", "ASC")
            fuelTypes.value = fuelTypeRepository.getFuelTypes(pageRequest)
        }
    }

    fun getFuelStationServices() {
        viewModelScope.launch {
            val pageRequest = PageRequest(1, 100, "Name", "ASC")
            fuelStationServices.value = fuelStationServiceRepository.getFuelStationServices(pageRequest)
        }
    }

    fun toggleFuelType(fuelTypeId: Long, checked: Boolean) {
        if (checked) {
            addFuelType(fuelTypeId)
        } else {
            removeFuelType(fuelTypeId)
        }

        isChange = true
        MapViewModelMediator.fuelStationChanged()
        ListViewModelMediator.fuelStationChanged()
    }

    private fun addFuelType(fuelTypeId: Long) {
        if (fuelStation == null) return

        viewModelScope.launch {
            val addFuelToStation = AddFuelToStation(fuelStation!!.id, fuelTypeId)
            addFuelType.value = fuelAtStationRepository.addFuelToStation(addFuelToStation)
        }
    }

    private fun removeFuelType(fuelTypeId: Long) {
        if (fuelStation == null) return

        viewModelScope.launch {
            removeFuelType.value = fuelAtStationRepository.deleteFuelFromStation(fuelStation!!.id, fuelTypeId)
        }
    }

    fun toggleService(serviceId: Long, checked: Boolean) {
        if (checked) {
            addService(serviceId)
        } else {
            removeService(serviceId)
        }

        isChange = true
        MapViewModelMediator.fuelStationChanged()
        ListViewModelMediator.fuelStationChanged()
    }

    private fun addService(serviceId: Long) {
        if (fuelStation == null) return

        viewModelScope.launch {
            val addServiceToStation = AddServiceToStation(fuelStation!!.id, serviceId)
            addService.value = serviceAtStationRepository.addServiceToStation(addServiceToStation)
        }
    }

    private fun removeService(serviceId: Long) {
        if (fuelStation == null) return

        viewModelScope.launch {
            removeService.value = serviceAtStationRepository.deleteServiceFromStation(fuelStation!!.id, serviceId)
        }
    }

    fun isFuelStationServiceSelected(serviceId: Long): Boolean =
        fuelStation?.services?.any { service -> service.id == serviceId } == true

    fun isFuelTypeSelected(fuelTypeId: Long): Boolean =
        fuelStation?.fuelTypes?.any { fuelTypes -> fuelTypes.id == fuelTypeId } == true

    fun setFuelStation(fuelStation: FuelStationDetails?) {
        this.fuelStation = fuelStation
    }

    fun isChange(): Boolean {
        return isChange
    }

    fun clear() {
        fuelTypes = MutableLiveData()
        fuelStationServices = MutableLiveData()

        addFuelType= MutableLiveData()
        removeFuelType = MutableLiveData()
        addService = MutableLiveData()
        removeService = MutableLiveData()
    }
}