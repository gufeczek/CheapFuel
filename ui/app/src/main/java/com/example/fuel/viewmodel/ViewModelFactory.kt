package com.example.fuel.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.repository.BlockUserRepository
import com.example.fuel.repository.FavouriteRepository
import com.example.fuel.repository.FuelAtStationRepository
import com.example.fuel.repository.FuelPriceRepository
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.FuelStationServiceRepository
import com.example.fuel.repository.FuelTypeRepository
import com.example.fuel.repository.ReportRepository
import com.example.fuel.repository.ReviewRepository
import com.example.fuel.repository.ServiceAtStationRepository
import com.example.fuel.repository.StationChainRepository
import com.example.fuel.repository.UserRepository
import com.example.fuel.utils.exception.NoSuchViewModelException

class ViewModelFactory : ViewModelProvider.Factory {

    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T = when(modelClass) {
        UserRegistrationViewModel::class.java -> UserRegistrationViewModel(UserRepository()) as T
        UserLoginViewModel::class.java -> UserLoginViewModel(UserRepository()) as T
        FuelStationMapViewModel::class.java -> FuelStationMapViewModel(
            FuelStationRepository(),
            FuelTypeRepository(),
            FuelStationServiceRepository(),
            StationChainRepository()) as T
        FuelStationDetailsViewModel::class.java -> FuelStationDetailsViewModel(
            FuelStationRepository(),
            ReviewRepository(),
            FavouriteRepository(),
            FuelPriceRepository(),
            ReportRepository()) as T
        FuelStationEditorViewModel::class.java -> FuelStationEditorViewModel(
            FuelAtStationRepository(),
            ServiceAtStationRepository(),
            FuelTypeRepository(),
            FuelStationServiceRepository()) as T
        FuelStationListViewModel::class.java -> FuelStationListViewModel(
            FuelStationRepository(),
            FuelTypeRepository(),
            FuelStationServiceRepository(),
            StationChainRepository()) as T
        FavouritesFuelStationsViewModel::class.java -> FavouritesFuelStationsViewModel(
            FavouriteRepository()) as T
        UserListViewModel::class.java -> UserListViewModel(
            UserRepository()) as T
        UserDetailsViewModel::class.java -> UserDetailsViewModel(
            UserRepository(),
            ReviewRepository(),
            BlockUserRepository()) as T
        CalculatorViewModel::class.java -> CalculatorViewModel(
            FuelStationRepository(),
            FuelTypeRepository()) as T
        NewFuelStationViewModel::class.java -> NewFuelStationViewModel(
            StationChainRepository(),
            FuelStationRepository(),
            FuelTypeRepository(),
            FuelStationServiceRepository()
        ) as T
        else -> {
            throw NoSuchViewModelException(
                "Exception in 'ViewModelFactory' class, 'create' method: such ViewModel doesn't exist"
            )
        }
    }
}