package com.example.fuel.ui.fragment

import android.os.Bundle
import android.util.Log
import android.util.Patterns
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetRegisterMethodBinding
import com.example.fuel.ui.utils.extension.EditTextExtension.Companion.afterTextChanged

class SetRegisterMethodFragment : Fragment(R.layout.fragment_set_register_method) {

    private lateinit var binding: FragmentSetRegisterMethodBinding

    private enum class ValidationErrors {
        ERROR_INCORRECT_EMAIL
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentSetRegisterMethodBinding.inflate(inflater, container, false)
        binding.btnRegister.setOnClickListener {
            if (!Patterns.EMAIL_ADDRESS.matcher(binding.etEmail.text.toString()).matches()) {
                //TODO: Remove this if statement later, only for testing purposes
                if(binding.etEmail.text.toString() == "") {
                    Navigation.findNavController(binding.root).navigate(R.id.setUsernameFragment)
                }
                binding.tilEmail.setBackgroundResource(R.drawable.bg_rounded_error)
                binding.tvEmailValidationError.text = ValidationErrors.ERROR_INCORRECT_EMAIL.toString()
                binding.etEmail.afterTextChanged { editable ->
                    if (Patterns.EMAIL_ADDRESS.matcher(editable).matches()) {
                        binding.tilEmail.setBackgroundResource(R.drawable.bg_rounded)
                        binding.tvEmailValidationError.text = ""
                    }
                }
            } else {
                Navigation.findNavController(binding.root).navigate(R.id.setUsernameFragment)
            }
        }
        return binding.root
    }
}