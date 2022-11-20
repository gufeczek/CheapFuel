package com.example.fuel.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.repository.FuelStationRepository
import com.example.fuel.repository.FuelStationServiceRepository
import com.example.fuel.repository.FuelTypeRepository
import com.example.fuel.repository.ReviewRepository
import com.example.fuel.repository.StationChainRepository
import com.example.fuel.repository.TestRepository
import com.example.fuel.repository.UserRepository
import com.example.fuel.utils.exception.NoSuchViewModelException

class ViewModelFactory : ViewModelProvider.Factory {

    @Suppress("UNCHECKED_CAST")
    override fun <T : ViewModel> create(modelClass: Class<T>): T = when(modelClass) {
        UserViewModel::class.java -> UserViewModel(UserRepository()) as T
        FuelStationMapViewModel::class.java -> FuelStationMapViewModel(
            FuelStationRepository(),
            FuelTypeRepository(),
            FuelStationServiceRepository(),
            StationChainRepository()) as T
        FuelStationDetailsViewModel::class.java -> FuelStationDetailsViewModel(
            FuelStationRepository(),
            ReviewRepository()) as T
        TestViewModel::class.java -> TestViewModel(TestRepository()) as T
        else -> {
            throw NoSuchViewModelException(
                "Exception in 'ViewModelFactory' class, 'create' method: such ViewModel doesn't exist"
            )
        }
    }
}