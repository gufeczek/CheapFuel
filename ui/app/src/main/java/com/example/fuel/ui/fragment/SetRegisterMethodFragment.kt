package com.example.fuel.ui.fragment

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentSetRegisterMethodBinding

class SetRegisterMethodFragment : Fragment(R.layout.fragment_set_register_method) {

    private lateinit var binding: FragmentSetRegisterMethodBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentSetRegisterMethodBinding.inflate(inflater, container, false)
        binding.btnRegister.setOnClickListener{
            Navigation.findNavController(binding.root).navigate(R.id.setUsernameFragment)
        }
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
    }
}