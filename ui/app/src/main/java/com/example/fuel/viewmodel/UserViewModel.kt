package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.User
import com.example.fuel.repository.UserRepository
import kotlinx.coroutines.launch
import retrofit2.Response

class UserViewModel(private val repository: UserRepository): ViewModel() {

    val response: MutableLiveData<Response<User>> = MutableLiveData()

    fun postLogin(user: User) {
        viewModelScope.launch {
            response.value = repository.postLogin(user)
        }
    }

    fun postRegister(user: User) {
        viewModelScope.launch {
            response.value = repository.postRegister(user)
        }
    }
}