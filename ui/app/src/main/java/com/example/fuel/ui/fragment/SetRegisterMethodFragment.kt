package com.example.fuel.ui.fragment

import android.content.Context
import android.os.Bundle
import android.util.Log
import android.util.Patterns
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModelProvider
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetRegisterMethodBinding
import com.example.fuel.model.User
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.utils.validation.ValidatorEmail
import com.example.fuel.viewmodel.UserViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class SetRegisterMethodFragment : Fragment(R.layout.fragment_set_register_method) {

    private var _binding: FragmentSetRegisterMethodBinding? = null
    private val binding get() = _binding!!
    private var error: ValidatorEmail.Error? = null
    private lateinit var viewModel: UserViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        retrofitTest()
        _binding = FragmentSetRegisterMethodBinding.inflate(inflater, container, false)

        binding.btnRegister.setOnClickListener(btnRegisterOnClickListener)
        binding.clMain.setOnClickListener { view -> view.hideKeyboard() }
        // TODO: Remove this, only for testing purposes
        binding.btnGoToMap.setOnClickListener {
            Navigation.findNavController(binding.root).navigate(R.id.mapFragment)
        }

        return binding.root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }


    private fun retrofitTest() {
        val user = User("sfafsfs",
            "xddds@gmail.com",
            "zaq1@WSX",
            "zaq1@WSX")
        val viewModelFactory = ViewModelFactory()
        viewModel = ViewModelProvider(this, viewModelFactory)[UserViewModel::class.java]
        viewModel.postRegister(user)
        viewModel.response.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                Log.d("XD", response.message())
            } else {
                Log.d("XD", response.code().toString())
            }
        }
    }

    private val btnRegisterOnClickListener = View.OnClickListener {
        val validator = ValidatorEmail(binding.etEmail.text.toString())
        if (!validator.validate()) {
            error = validator.error
            //TODO: Remove this if statement later, only for testing purposes
            if(binding.etEmail.text.toString() == "") {
                Navigation.findNavController(binding.root).navigate(R.id.setUsernameFragment)
            }
            binding.tilEmail.setBackgroundResource(R.drawable.bg_rounded_error)
            binding.tvEmailValidationError.text = ValidatorEmail.Error.ERROR_INCORRECT_EMAIL.toString()
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

    override fun onAttach(context: Context) {
        super.onAttach(context)

        (activity as AppCompatActivity).supportActionBar?.hide()
    }

    override fun onResume() {
        super.onResume()

        (activity as AppCompatActivity).supportActionBar?.hide()
    }
}