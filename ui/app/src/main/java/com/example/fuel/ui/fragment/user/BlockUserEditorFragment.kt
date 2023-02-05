package com.example.fuel.ui.fragment.user

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import com.example.fuel.databinding.FragmentBlockUserEditorBinding
import com.example.fuel.viewmodel.UserDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class BlockUserEditorFragment : Fragment() {
    private lateinit var viewModel: UserDetailsViewModel
    private lateinit var binding: FragmentBlockUserEditorBinding
    private var username: String? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserDetailsViewModel::class.java]
        binding = FragmentBlockUserEditorBinding.inflate(inflater, container, false)

        username = requireArguments().getString("username")

        initBlockButton()

        return binding.root
    }

    private fun initBlockButton() {
        val button = binding.mbBlockUser

        button.setOnClickListener {
            val content = getContent()

            viewModel.blockUser(username!!, content)
            findNavController().popBackStack()
        }
    }

    private fun getContent(): String = binding.tietBlockContent.text.toString()
}