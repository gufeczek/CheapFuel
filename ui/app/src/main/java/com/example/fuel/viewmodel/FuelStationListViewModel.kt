package com.example.fuel.viewmodel

import android.content.res.Resources
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.R
import com.example.fuel.model.FuelStationPageRequest
import com.example.fuel.model.FuelStationsFilter
import com.example.fuel.model.FuelType
import com.example.fuel.model.SimpleFuelStation
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.FuelTypeRepository
import com.example.fuel.utils.converters.Converter
import kotlinx.coroutines.launch
import retrofit2.Response
import java.time.Duration
import java.time.LocalDateTime
import java.util.*

class FuelStationListViewModel(
    private val fuelStationRepository: FuelStationRepository,
    private val fuelTypeRepository: FuelTypeRepository): ViewModel() {

    var fuelTypes: MutableLiveData<Response<Page<FuelType>>> = MutableLiveData()
    var fuelStations: MutableLiveData<Response<Page<SimpleFuelStation>>> = MutableLiveData()

    private var isFuelStationInitialized = false

    private var _filter: FuelStationsFilter? = null
    val filter get() = _filter!!

    private var currentFilter: FuelStationsFilter? = null

    fun getFirstPageOfFuelStations() {
        viewModelScope.launch {
            val result = initFilter()

            if (result) {
                getFirstPageOfFuelStationsIfFilterChanged()
            }
        }
    }

    private suspend fun initFilter(): Boolean {
        if (_filter == null) {
            val pageRequest = PageRequest(1, 1, "Name", "ASC")
            val fuelTypes = fuelTypeRepository.getFuelTypes(pageRequest).body()

            if (fuelTypes == null || fuelTypes.data.isEmpty()) return false

            initFilter(fuelTypes.data[0].id)
        }
        return true
    }

    private suspend fun getFirstPageOfFuelStationsIfFilterChanged() {
        if (filter != currentFilter) {
            currentFilter = filter.copy()
            val pageRequest = PageRequest(1, 10, "id", "Desc")

            fuelStations.value = fuelStationRepository.getSimpleListFuelStations(FuelStationPageRequest(filter, pageRequest))
        }
    }

    fun getNextPageOfFuelStations() {
        viewModelScope.launch {
            val nextPage = (if (fuelStations.value != null) fuelStations.value?.body()?.nextPage else 1) ?: return@launch

            val pageRequest = PageRequest(nextPage, 10, "CreatedAt", "Desc")
            fuelStations.value = fuelStationRepository.getSimpleListFuelStations(FuelStationPageRequest(currentFilter!!, pageRequest))
        }
    }

    fun initFilter(fuelTypeId: Long) {
        if (!isFuelStationInitialized) {
            _filter = _filter ?: FuelStationsFilter(fuelTypeId, null, null, null, null)
            isFuelStationInitialized = true
        }
    }

    fun willDataChange(): Boolean {
        return _filter == null || _filter != currentFilter
    }

    fun hasMoreFuelStations(): Boolean = fuelStations.value?.body()?.nextPage != null

    fun parsePrice(fuelPrice: Double, resources: Resources): String {
        return resources.getString(R.string.zloty, Converter.toCurrency(fuelPrice))
    }

    fun parseFuelPriceLastUpdate(lastUpdate: Date?, resources: Resources): String {
        if (lastUpdate == null) return resources.getString(R.string.never)

        val createdAt = Converter.toLocalDateTime(lastUpdate)
        val diff = Duration.between(createdAt, LocalDateTime.now())

        if (diff.toDays() == 1L) return resources.getString(R.string.one_day_ago)
        if (diff.toDays() > 1) return resources.getString(R.string.days_ago, diff.toDays().toString())
        if (diff.toHours() > 1) return resources.getString(R.string.hours_ago, diff.toHours().toString())

        return resources.getString(R.string.one_hour_ago)
    }

    fun clear() {
        fuelTypes = MutableLiveData()
        fuelStations = MutableLiveData()

        isFuelStationInitialized = false
        _filter = null
        currentFilter = null
    }
}