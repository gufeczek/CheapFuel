package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelStationsFilter
import com.example.fuel.model.FuelType
import com.example.fuel.model.SimpleMapFuelStation
import com.example.fuel.model.StationChain
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.FuelStationServiceRepository
import com.example.fuel.repository.FuelTypeRepository
import com.example.fuel.repository.StationChainRepository
import kotlinx.coroutines.launch
import retrofit2.Response

private const val INITIAL_MIN_PRICE = 0F
private const val INITIAL_MAX_PRICE = 15F

private const val PRICE_COLOR_GREEN = -15624075
private const val PRICE_COLOR_RED = -4774081
private const val PRICE_COLOR_GREY = -7829368

class FuelStationMapViewModel(
    private val fuelStationRepository: FuelStationRepository,
    private val fuelTypeRepository: FuelTypeRepository,
    private val fuelStationServiceRepository: FuelStationServiceRepository,
    private val stationChainRepository: StationChainRepository): ViewModel() {

    val fuelTypes: MutableLiveData<Response<Page<FuelType>>> = MutableLiveData()
    val fuelStationServices: MutableLiveData<Response<Page<FuelStationService>>> = MutableLiveData()
    val stationChains: MutableLiveData<Response<Page<StationChain>>> = MutableLiveData()
    val fuelStations: MutableLiveData<Response<Array<SimpleMapFuelStation>>> = MutableLiveData()

    private var isFuelStationInitialized = false
    private var fuelStatistics: FuelStatistics? = null

    private var _filter: FuelStationsFilter? = null
    val filter get() = _filter!!

    private var currentFilter: FuelStationsFilter? = null

    fun getFuelStations() {
        viewModelScope.launch {
            if (initFilter()) {
                fetchFuelStationsIfFilterChanged()
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

    private suspend fun fetchFuelStationsIfFilterChanged() {
        if (filter != currentFilter) {
            currentFilter = filter.copy()
            fuelStations.value = fuelStationRepository.getSimpleMapFuelStations(filter)
        }
    }

    fun willDataChange(): Boolean {
        return _filter == null || _filter != currentFilter
    }

    fun initDataForFilter() {
        viewModelScope.launch {
            val pageRequest = PageRequest(1, 100, "Name", "ASC")
            fuelTypes.value = fuelTypeRepository.getFuelTypes(pageRequest)
            fuelStationServices.value = fuelStationServiceRepository.getFuelStationServices(pageRequest)
            stationChains.value = stationChainRepository.getStationChains(pageRequest)
        }
    }

    fun initFilter(fuelTypeId: Long) {
        if (!isFuelStationInitialized) {
            _filter = _filter ?: FuelStationsFilter(fuelTypeId, null, null, null, null)
            isFuelStationInitialized = true
        }
    }

    fun currentMinPrice(): Float {
        return _filter?.minPrice?.toFloat() ?: INITIAL_MIN_PRICE
    }

    fun currentMaxPrice(): Float {
        return _filter?.maxPrice?.toFloat() ?: INITIAL_MAX_PRICE
    }

    fun onFuelTypeSelected(fuelTypeId: Long) {
        _filter = _filter ?: FuelStationsFilter(fuelTypeId, null, null, null, null)

        _filter?.let {
            filter.fuelTypeId = fuelTypeId
            println(filter)
        }
    }

    fun onStationChainSelected(stationChainId: Long) {
        _filter?.let {
            filter.stationChainsIds = filter.stationChainsIds ?: mutableListOf()
            addOrRemove(filter.stationChainsIds!!, stationChainId)
            println(filter)
        }
    }

    fun onFuelStationServiceSelected(serviceId: Long) {
        _filter?.let {
            filter.servicesIds = filter.servicesIds ?: mutableListOf()
            addOrRemove(filter.servicesIds!!, serviceId)
            println(filter)
        }
    }

    fun onPriceRangeChanged(minPrice: Float, maxPrice: Float) {
        _filter?.let {
            filter.minPrice = minPrice.toDouble()
            filter.maxPrice = maxPrice.toDouble()
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

    fun calculateStatistics(fuelStations: Array<SimpleMapFuelStation>?) {
        if (fuelStations != null && fuelStations.size > 10) {
            fuelStations.sortedByDescending { it.price }
            val threshold = (fuelStations.size * 0.01).toInt()
            var minPriceBoundary = fuelStations.size - threshold;
            var maxPriceBoundary = threshold

            if (minPriceBoundary >= fuelStations.size) minPriceBoundary = fuelStations.size - 1
            if (maxPriceBoundary < 0) maxPriceBoundary = 0

            fuelStatistics = FuelStatistics(
                fuelStations[minPriceBoundary].price,
                fuelStations[maxPriceBoundary].price)
        }
    }

    fun getPriceColor(fuelStation: SimpleMapFuelStation): Int {
        if (fuelStatistics == null) return PRICE_COLOR_GREY

        if (fuelStation.price > fuelStatistics!!.maxPriceThreshold) {
            return PRICE_COLOR_RED
        } else if (fuelStation.price < fuelStatistics!!.minPriceThreshold){
            return PRICE_COLOR_GREEN
        }

        return PRICE_COLOR_GREY
    }

    fun shouldBeBold(fuelStation: SimpleMapFuelStation): Boolean {
        val color = getPriceColor(fuelStation)
        return color == PRICE_COLOR_RED || color == PRICE_COLOR_GREEN
    }

    private data class FuelStatistics(
        val minPriceThreshold: Double,
        val maxPriceThreshold: Double)
}