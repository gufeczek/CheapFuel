package com.example.fuel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.repository.Repository

class MainViewModelFactory(
    private val repository: Repository
) : ViewModelProvider.Factory {

}