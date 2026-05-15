﻿using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VizsgaBiztos.UI;

namespace UI
{
    public partial class LoginForm : Form
    {
        private readonly IUserService _userService;
        public LoginForm(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                btnLogin.Enabled = false;

                var user = await _userService.Authenticate(txtEmail.Text.Trim(), txtPassword.Text);

                MainForm mainForm = Program.ServiceProvider.GetRequiredService<MainForm>();
                mainForm.InitializeUser();  
                mainForm.Show();
                this.Hide();
            }
            catch (InvalidOperationException ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
                btnLogin.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Egy váratlan hiba történt: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
            }
        }
    }
}
