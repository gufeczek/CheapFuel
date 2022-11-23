package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.Test
import com.example.fuel.model.User
import com.example.fuel.repository.TestRepository
import com.example.fuel.repository.UserRepository
import kotlinx.coroutines.launch
import retrofit2.Response

class TestViewModel(private val repository: TestRepository) : ViewModel(){
    val response: MutableLiveData<Response<Test>> = MutableLiveData()

    fun getTest() {
        viewModelScope.launch {
            response.value = repository.getTest()
        }
    }
}