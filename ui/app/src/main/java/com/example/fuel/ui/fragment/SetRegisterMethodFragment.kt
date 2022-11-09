package com.example.fuel.ui.fragment

import android.content.Context
import android.os.Bundle
import android.util.Patterns
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetRegisterMethodBinding
import com.example.fuel.ui.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.ui.utils.extension.EditTextExtension.Companion.afterTextChanged

class SetRegisterMethodFragment : Fragment(R.layout.fragment_set_register_method) {

    private lateinit var binding: FragmentSetRegisterMethodBinding

    private enum class Error {
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
                binding.tvEmailValidationError.text = Error.ERROR_INCORRECT_EMAIL.toString()
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

        // TODO: Remove this, only for testing purposes
        binding.btnGoToMap.setOnClickListener {
            Navigation.findNavController(binding.root).navigate(R.id.mapFragment);
        }

        binding.clMain.setOnClickListener { view ->
            view.hideKeyboard()
        }

        return binding.root
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)

        (activity as AppCompatActivity).supportActionBar?.hide()
    }

    override fun onResume() {
        super.onResume()

        (activity as AppCompatActivity).supportActionBar?.hide()
    }
}