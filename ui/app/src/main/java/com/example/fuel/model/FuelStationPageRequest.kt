package com.example.fuel.model

import com.example.fuel.model.page.PageRequest

data class FuelStationPageRequest(
    val filter: FuelStationsFilter,
    val pageRequest: PageRequest
)
