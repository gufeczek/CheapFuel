package com.example.fuel.viewmodel

import android.view.View
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.navigation.NavDirections
import androidx.navigation.Navigation
import com.auth0.android.jwt.JWT
import com.example.fuel.R
import com.example.fuel.enums.Role
import com.example.fuel.model.Email
import com.example.fuel.model.Token
import com.example.fuel.model.account.UserLogin
import com.example.fuel.model.account.UserPasswordReset
import com.example.fuel.repository.UserRepository
import com.example.fuel.utils.Auth
import com.example.fuel.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.utils.validation.ValidatorEmail
import com.example.fuel.utils.validation.ValidatorPassword
import com.example.fuel.utils.validation.ValidatorToken
import com.example.fuel.utils.validation.ValidatorUsername
import com.google.android.material.textfield.TextInputEditText
import kotlinx.coroutines.launch
import retrofit2.Response

class UserLoginViewModel(private val repository: UserRepository) : ViewModel() {
    var response: MutableLiveData<Response<Token>> = MutableLiveData()
    var token: MutableLiveData<String> = MutableLiveData()
    var isTokenGenerated: MutableLiveData<Boolean> = MutableLiveData()
    var isPasswordReset: MutableLiveData<Boolean> = MutableLiveData()

    fun postLogin(user: UserLogin) {
        viewModelScope.launch {
            response.value = repository.postLogin(user)
        }
    }

    fun afterLogin() {
        token.value = response.value!!.body()?.token
        if (token.value != null) {
            setupAuth()
        }
    }

    private fun setupAuth() {
        val jwt = JWT(token.value.toString())
        Auth.username = jwt.getClaim("nameid").asString()!!
        Auth.role = Role.fromString(jwt.getClaim("role").asString()!!)
        Auth.token = "Bearer $jwt"
    }

    fun getPasswordResetToken(email: Email) {
        viewModelScope.launch {
            isTokenGenerated.value = repository.postPasswordResetToken(email).code().toString() == "200"
        }
    }

    fun postResetPassword(userPasswordReset: UserPasswordReset) {
        viewModelScope.launch {
            isPasswordReset.value = repository.postResetPassword(userPasswordReset).code().toString() == "200"
        }
    }

    fun getUsernameValidationError(username: String): ValidatorUsername.Error? {
        val validator = ValidatorUsername(username)
        validator.validate()
        return validator.error
    }

    fun getPasswordValidationError(password: String): ValidatorPassword.Error? {
        val validator = ValidatorPassword(password, password)
        validator.validate()
        return validator.error
    }

    fun getPasswordValidationError(password: String, repeatPassword: String): ValidatorPassword.Error? {
        val validator = ValidatorPassword(password, repeatPassword)
        validator.validate()
        return validator.error
    }

    fun getEmailValidationError(email: String): ValidatorEmail.Error? {
        val validator = ValidatorEmail(email)
        validator.validate()
        return validator.error
    }

    fun getTokenValidationError(token: Array<TextInputEditText>): ValidatorToken.Error? {
        val validator = ValidatorToken(token)
        validator.validate()
        return validator.error
    }

    fun navigateToResetPasswordFragment(view: View) {
        Navigation.findNavController(view).navigate(R.id.resetPassword)
    }

    fun navigateToResetPasswordCodeFragment(view: View, action: NavDirections) {
        Navigation.findNavController(view).navigate(action)
    }

    fun navigateToLoginFragment(view: View) {
        Navigation.findNavController(view).navigate(R.id.LoginFragment)
    }

    fun navigateToFuelStatonListFragment(view: View) {
        Navigation.findNavController(view).navigate(R.id.fuelStationListFragment)
    }

    fun setupCodeInputLogic(codes: Array<TextInputEditText>) {
        for (i in codes.indices) {
            if (i != codes.size - 1) {
                codes[i].afterTextChanged {
                    if (codes[i].text.toString() != "") {
                        codes[i + 1].requestFocus()
                    }
                }
            }
        }
    }
}