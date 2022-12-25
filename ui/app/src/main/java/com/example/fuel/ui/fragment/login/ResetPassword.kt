package com.example.fuel.ui.fragment.login

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentLoginBinding
import com.example.fuel.databinding.FragmentResetPasswordBinding
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.validation.ValidatorPassword
import com.example.fuel.utils.validation.ValidatorUsername
import com.example.fuel.viewmodel.UserLoginViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class ResetPassword : Fragment() {

    private var _binding: FragmentResetPasswordBinding? = null
    private val binding get() = _binding!!
    private var error: ValidatorPassword.Error? = null
    private lateinit var viewModel: UserLoginViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentResetPasswordBinding.inflate(inflater, container, false)
        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserLoginViewModel::class.java]

        binding.clMain.setOnClickListener { view -> view.hideKeyboard() }
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }

        return binding.root
    }

}