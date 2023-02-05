package com.example.fuel.viewmodel.mediator

import com.example.fuel.viewmodel.FavouritesFuelStationsViewModel

object FavouriteViewModelMediator {
    private var favouriteViewModel: FavouritesFuelStationsViewModel? = null
    private var changeDetected = false

    fun subscribe(favouriteViewModel: FavouritesFuelStationsViewModel) {
        this.favouriteViewModel = favouriteViewModel
    }

    fun unsubscribe() {
        this.favouriteViewModel = null
        changeDetected = false
    }

    fun favouriteChanged() {
        changeDetected = true
    }

    fun act() {
        if (changeDetected && favouriteViewModel != null) {
            favouriteViewModel!!.getFirstPageOfFavourites()
        }
    }
}