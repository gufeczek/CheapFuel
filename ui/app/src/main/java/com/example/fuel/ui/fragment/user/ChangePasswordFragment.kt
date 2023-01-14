package com.example.fuel.ui.fragment.user

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import com.example.fuel.databinding.FragmentChangePasswordBinding
import com.example.fuel.model.account.ChangePassword
import com.example.fuel.viewmodel.UserDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class ChangePasswordFragment : Fragment() {
    private lateinit var viewModel: UserDetailsViewModel
    private lateinit var binding: FragmentChangePasswordBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserDetailsViewModel::class.java]
        binding = FragmentChangePasswordBinding.inflate(inflater, container, false)

        initChangePasswordButton()

        return binding.root
    }

    private fun initChangePasswordButton() {
        val button = binding.mbReset

        button.setOnClickListener {
            val changePassword = ChangePassword(
                binding.tietOldPassword.text.toString(),
                binding.tietNewPassword.text.toString(),
                binding.tietConfirmPassword.text.toString()
            )

            viewModel.changePassword(changePassword)
            findNavController().popBackStack()
        }
    }
}