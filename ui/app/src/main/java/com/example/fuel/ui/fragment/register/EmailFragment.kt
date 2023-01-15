package com.example.fuel.ui.fragment.register

import android.content.Context
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModelProvider
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentEmailBinding
import com.example.fuel.utils.extension.ContextExtension.Companion.hideKeyboard
import com.example.fuel.utils.extension.EditTextExtension.Companion.afterTextChanged
import com.example.fuel.utils.validation.ValidatorEmail
import com.example.fuel.viewmodel.UserRegistrationViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class EmailFragment : Fragment(R.layout.fragment_email) {

    private var _binding: FragmentEmailBinding? = null
    private val binding get() = _binding!!
    private var error: ValidatorEmail.Error? = null
    private lateinit var viewModel: UserRegistrationViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentEmailBinding.inflate(inflater, container, false)
        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserRegistrationViewModel::class.java]

        binding.btnRegister.setOnClickListener(btnGoToNextPage)
        binding.clMain.setOnClickListener { view -> view.hideKeyboard() }
        binding.btnLogin.setOnClickListener { view ->
            viewModel.navigateToLoginFragment(view)
        }

        return binding.root
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)
        (activity as AppCompatActivity).supportActionBar?.hide()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private val btnGoToNextPage = View.OnClickListener { view ->
        val email = binding.etEmail.text.toString()
        error = viewModel.getEmailValidationError(email)
        if (error == null) {
            val action = EmailFragmentDirections.navigateToUsernameFragment(email)
            viewModel.navigateToUsernameFragment(view, action)

        } else {
            showError()
            setEmailErrorTracking()
        }
    }

    private fun showError() {
        binding.tilEmail.setBackgroundResource(R.drawable.bg_rounded_error)
        binding.tvEmailValidationError.text = error.toString()
    }

    private fun hideError() {
        binding.tilEmail.setBackgroundResource(R.drawable.bg_rounded)
        binding.tvEmailValidationError.text = ""
    }

    private fun setEmailErrorTracking() {
        binding.etEmail.afterTextChanged { email ->
            if(viewModel.getEmailValidationError(email) == null) {
                hideError()
                stopEmailErrorTracking()
            }
        }
    }

    private fun stopEmailErrorTracking() {
        binding.etEmail.afterTextChanged {}
    }
}