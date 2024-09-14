using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System;

namespace Messanger
{
    partial class Messanger
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Timer TickTimer;
            this.SendButton = new System.Windows.Forms.Button();
            this.Input = new System.Windows.Forms.RichTextBox();
            this.Messages = new System.Windows.Forms.RichTextBox();
            this.choiseFile = new System.Windows.Forms.OpenFileDialog();
            TickTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // TickTimer
            // 
            TickTimer.Enabled = true;
            TickTimer.Tick += new System.EventHandler(this.Recive);
            // 
            // SendButton
            // 
            this.SendButton.BackColor = System.Drawing.Color.Coral;
            this.SendButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.SendButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkSalmon;
            this.SendButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            this.SendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SendButton.Location = new System.Drawing.Point(624, 403);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(138, 35);
            this.SendButton.TabIndex = 0;
            this.SendButton.Text = "Отправить";
            this.SendButton.UseVisualStyleBackColor = false;
            this.SendButton.Click += new System.EventHandler(this.Send);
            // 
            // Input
            // 
            this.Input.BackColor = System.Drawing.Color.Coral;
            this.Input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Input.Location = new System.Drawing.Point(12, 403);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(606, 35);
            this.Input.TabIndex = 1;
            this.Input.Text = "";
            this.Input.Enter += new System.EventHandler(this.InputOnGotFocus);
            this.Input.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnMessage);
            this.Input.Leave += new System.EventHandler(this.InputOnLostFocus);
            // 
            // Messages
            // 
            this.Messages.BackColor = System.Drawing.Color.Coral;
            this.Messages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Messages.Location = new System.Drawing.Point(12, 12);
            this.Messages.Name = "Messages";
            this.Messages.ReadOnly = true;
            this.Messages.Size = new System.Drawing.Size(750, 385);
            this.Messages.TabIndex = 3;
            this.Messages.Text = "Ваше имя пользователя:\n";
            this.Messages.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.OnClickLink);
            // 
            // choiseFile
            // 
            this.choiseFile.FileName = "openFileDialog1";
            this.choiseFile.Filter = "Текст|*.txt|Все|*.*";
            // 
            // Messanger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OrangeRed;
            this.ClientSize = new System.Drawing.Size(772, 450);
            this.Controls.Add(this.Messages);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.SendButton);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "Messanger";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StopConnections);
            this.Load += new System.EventHandler(this.Start);
            this.Resize += new System.EventHandler(this.ResizeWindow);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button SendButton;
        protected System.Windows.Forms.RichTextBox Input;
        private System.Windows.Forms.RichTextBox Messages;
        private OpenFileDialog choiseFile;
    }
}

