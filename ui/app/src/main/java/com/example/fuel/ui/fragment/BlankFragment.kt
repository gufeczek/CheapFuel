package com.example.fuel.ui.fragment

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.example.fuel.R
import com.example.fuel.databinding.FragmentBlankBinding

class BlankFragment : Fragment(R.layout.fragment_blank) {

    private lateinit var binding: FragmentBlankBinding

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        binding = FragmentBlankBinding.inflate(inflater, container, false)

        return binding.root
    }

}