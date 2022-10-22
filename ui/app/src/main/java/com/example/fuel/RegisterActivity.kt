package com.example.fuel

import android.content.res.ColorStateList
import android.graphics.Color
import android.graphics.drawable.ColorDrawable
import android.graphics.drawable.Drawable
import android.graphics.drawable.PaintDrawable
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.EditText
import android.widget.Toolbar
import androidx.appcompat.app.AppCompatActivity

class RegisterActivity : AppCompatActivity() {

    private lateinit var etPassword: EditText
    private lateinit var toolbar: Toolbar
    private lateinit var btnNextPage: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)
        supportActionBar?.hide()

        etPassword = findViewById(R.id.registerActivity_et_password)
        toolbar = findViewById(R.id.registerActivity_toolbar)
        btnNextPage = findViewById(R.id.registerActivity_btnNextPage)

        CornerDrawable.roundCorners(etPassword, 20f)

        toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
    }
}