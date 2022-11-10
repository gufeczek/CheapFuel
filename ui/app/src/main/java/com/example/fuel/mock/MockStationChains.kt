package com.example.fuel.mock

import com.example.fuel.model.StationChain

fun getStationChains(): Array<StationChain> {
    return arrayOf(
        StationChain(1, "Orlen"),
        StationChain(2, "Lotos"),
        StationChain(3, "CircleK"),
        StationChain(4, "BP"),
        StationChain(5, "Shell")
    )
}