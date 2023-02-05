package com.example.fuel.viewmodel

import android.content.res.Resources
import android.util.Log
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.R
import com.example.fuel.model.FuelStationFilterWithLocation
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelType
import com.example.fuel.model.SimpleFuelStation
import com.example.fuel.model.StationChain
import com.example.fuel.model.UserLocation
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.model.page.SortOption
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.FuelStationServiceRepository
import com.example.fuel.repository.FuelTypeRepository
import com.example.fuel.repository.StationChainRepository
import com.example.fuel.utils.converters.Converter
import com.example.fuel.viewmodel.mediator.ListViewModelMediator
import kotlinx.coroutines.launch
import retrofit2.Response
import java.time.Duration
import java.time.LocalDateTime
import java.util.*

private const val INITIAL_MIN_PRICE = 0F
private const val INITIAL_MAX_PRICE = 15F
private const val INITIAL_DISTANCE = 500F

class FuelStationListViewModel(
    private val fuelStationRepository: FuelStationRepository,
    private val fuelTypeRepository: FuelTypeRepository,
    private val fuelStationServiceRepository: FuelStationServiceRepository,
    private val stationChainRepository: StationChainRepository): ViewModel() {

    var fuelTypes: MutableLiveData<Response<Page<FuelType>>> = MutableLiveData()
    var fuelStationServices: MutableLiveData<Response<Page<FuelStationService>>> = MutableLiveData()
    var stationChains: MutableLiveData<Response<Page<StationChain>>> = MutableLiveData()
    var fuelStations: MutableLiveData<Response<Page<SimpleFuelStation>>> = MutableLiveData()

    private var isFuelStationInitialized = false

    private var sortOptions = arrayOf(
        SortOption("Price", "ASC"),
        SortOption("Price", "DESC"),
        SortOption("Updated", "DESC"),
        SortOption("Distance", "ASC")
    )
    private var selectedSort: Int? = null
    private var currentSort: Int? = null

    private var _filter: FuelStationFilterWithLocation? = null
    val filter get() = _filter!!

    private var currentFilter: FuelStationFilterWithLocation? = null
    private var userLocation: UserLocation? = null

    private var hardReload = false

    fun getFirstPageOfFuelStations() {
        viewModelScope.launch {
            val result = initFilter()

            if (hardReload) {
                hardReload = false
                fetchFirstPageOfFuelStations()
                return@launch
            }

            if (result) {
                initSort()
                getFirstPageOfFuelStationsIfFilterChanged()
            }
        }
    }

    private suspend fun initFilter(): Boolean {
        if (_filter == null) {
            val pageRequest = PageRequest(1, 1, "Id", "ASC")
            val fuelTypes = fuelTypeRepository.getFuelTypes(pageRequest).body()

            if (fuelTypes == null || fuelTypes.data.isEmpty()) return false

            initFilter(fuelTypes.data[0].id)
        }
        return true
    }

    private fun initSort() {
        if (selectedSort == null) {
            selectedSort = 0
        }
    }

    private suspend fun getFirstPageOfFuelStationsIfFilterChanged() {
        if (filter != currentFilter || currentSort != selectedSort) {
            currentFilter = filter.copy()
            currentSort = selectedSort

            val currentSort = getSort()
            val pageRequest = PageRequest(1, 10, currentSort.property, currentSort.direction)

            fuelStations.value = fuelStationRepository.getSimpleListFuelStations(filter, pageRequest)
        }
    }

    fun getNextPageOfFuelStations() {
        viewModelScope.launch {
            val currentSort = getSort()
            val nextPage = (if (fuelStations.value != null) fuelStations.value?.body()?.nextPage else 1) ?: return@launch

            val pageRequest = PageRequest(nextPage, 10, currentSort.property, currentSort.direction)
            fuelStations.value = fuelStationRepository.getSimpleListFuelStations(currentFilter!!, pageRequest)
        }
    }

    private fun fetchFirstPageOfFuelStations() {
        viewModelScope.launch {
            val currentSort = getSort()
            val pageRequest = PageRequest(1, 10, currentSort.property, currentSort.direction)

            fuelStations.value = fuelStationRepository.getSimpleListFuelStations(currentFilter!!, pageRequest)
        }
    }

    private fun getSort() = sortOptions[currentSort ?: 0]

    fun initFilter(fuelTypeId: Long) {
        if (!isFuelStationInitialized) {
            _filter = _filter ?: FuelStationFilterWithLocation(fuelTypeId, null, null, null, null, null, null, null)

            if (userLocation != null) {
                filter.userLatitude = userLocation!!.latitude
                filter.userLongitude = userLocation!!.longitude
            }

            isFuelStationInitialized = true
        }
    }

    fun initDataForFilter() {
        viewModelScope.launch {
            val pageRequest = PageRequest(1, 100, "Id", "ASC")
            fuelTypes.value = fuelTypeRepository.getFuelTypes(pageRequest)
            fuelStationServices.value = fuelStationServiceRepository.getFuelStationServices(pageRequest)
            stationChains.value = stationChainRepository.getStationChains(pageRequest)
        }
    }

    fun willDataChange(): Boolean {
        return _filter == null || _filter != currentFilter || selectedSort != currentSort
    }

    fun hasMoreFuelStations(): Boolean = fuelStations.value?.body()?.nextPage != null

    fun hasAnyFuelStations(): Boolean = fuelStations.value?.body()?.totalElements != null
            && fuelStations.value!!.body()!!.totalElements > 0

    fun isDistanceSet(): Boolean = _filter?.distance != null

    fun currentMinPrice(): Float {
        return _filter?.minPrice?.toFloat() ?: INITIAL_MIN_PRICE
    }

    fun currentMaxPrice(): Float {
        return _filter?.maxPrice?.toFloat() ?: INITIAL_MAX_PRICE
    }

    fun currentDistance(): Float {
        return _filter?.distance?.toFloat()?.div(1000) ?: INITIAL_DISTANCE
    }

    fun onFuelTypeSelected(fuelTypeId: Long) {
        _filter = _filter ?: FuelStationFilterWithLocation(
            fuelTypeId, null, null, null, null,
            userLocation?.longitude, userLocation?.latitude, null)

        _filter?.let {
            filter.fuelTypeId = fuelTypeId
        }
    }

    fun onStationChainSelected(stationChainId: Long) {
        _filter?.let {
            filter.stationChainsIds = filter.stationChainsIds ?: mutableListOf()
            addOrRemove(filter.stationChainsIds!!, stationChainId)
        }
    }

    fun onFuelStationServiceSelected(serviceId: Long) {
        _filter?.let {
            filter.servicesIds = filter.servicesIds ?: mutableListOf()
            addOrRemove(filter.servicesIds!!, serviceId)
        }
    }

    fun onPriceRangeChanged(minPrice: Float, maxPrice: Float) {
        _filter?.let {
            filter.minPrice = minPrice.toDouble()
            filter.maxPrice = maxPrice.toDouble()
        }
    }

    fun onDistanceChange(distance: Float?) {
        _filter?.let {
            filter.distance = distance?.toDouble()?.times(1000)
        }
    }

    private fun addOrRemove(list: MutableList<Long>, value: Long) {
        if (list.contains(value)) {
            list.removeAll { it == value }
        } else {
            list.add(value)
        }
    }

    fun isFuelTypeSelected(fuelTypeId: Long): Boolean {
        return _filter?.fuelTypeId == fuelTypeId
    }

    fun isStationChainSelected(stationChainId: Long): Boolean {
        return _filter?.stationChainsIds?.contains(stationChainId) == true
    }

    fun isFuelStationServiceSelected(serviceId: Long): Boolean {
        return _filter?.servicesIds?.contains(serviceId) == true
    }

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

    fun setUserLocation(lat: Double, lon: Double) {
        userLocation = UserLocation(lat, lon)
    }

    fun isUserLocationSet(): Boolean = userLocation != null

    fun sortOptions(): Array<String> {
        return if (userLocation != null) {
            arrayOf("Najtańsze paliwo", "Najdroższe paliwo", "Ostatnio zaktualizowane", "Odległość")
        } else {
            arrayOf("Najtańsze paliwo", "Najdroższe paliwo", "Ostatnio zaktualizowane")
        }
    }

    fun isFirstPage(): Boolean = fuelStations.value?.body()?.pageNumber == 1

    fun choiceSort(idx: Int) {
        selectedSort = idx
    }

    fun cancelSort() {
        selectedSort = currentSort
    }

    fun currentSort(): Int = currentSort!!


    fun getFuelTypeId(): Long {
        return _filter?.fuelTypeId ?: 1
    }

    fun hardReload() {
        hardReload = true
    }

    fun init() {
        ListViewModelMediator.subscribe(this)
    }

    fun clear() {
        fuelTypes = MutableLiveData()
        fuelStations = MutableLiveData()

        isFuelStationInitialized = false
        _filter = null
        currentFilter = null

        ListViewModelMediator.unsubscribe()
    }
}

