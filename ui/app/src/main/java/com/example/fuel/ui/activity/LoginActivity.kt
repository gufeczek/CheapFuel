package com.example.fuel.ui.activity

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import com.example.fuel.R
import com.example.fuel.ui.fragment.SetRegisterMethodFragment

class LoginActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)
        supportActionBar?.hide()

        supportFragmentManager
            .beginTransaction()
            .replace(R.id.frameLayout2, SetRegisterMethodFragment())
            .commit()

    }
}