using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Xamarin.Forms;

namespace CapturandoImagem
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        //imagem usada na captura
        byte[] img = null;


        //Conversor Imagem -> Bytes
        public byte[] ReadFully (Stream Imput)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while((read=Imput.Read(buffer,0,buffer.Length))>0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
                
        }

        private async void btnGaleria_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Indísponível", "Recurso indisponível", "Ok");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,                    
                });

                if (file == null)
                {
                    return;
                }

                img = ReadFully(file.GetStream());
                imgfoto.Source = ImageSource.FromStream(() => file.GetStream());
            }

            catch(Exception ex)
            {
                await this.DisplayAlert("Erro", "Problema ao Executar ação", "ok");
            }
        }

        private async void btnCapturar_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Sem Camera", "Recurso indisponível", "Ok");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Full,
                    AllowCropping=true,
                    SaveToAlbum = true,
                    Name = "photo.jpg",
                    DefaultCamera = CameraDevice.Front,
                });

                if (file == null)
                {
                    return;
                }

                img = ReadFully(file.GetStream());
                imgfoto.Source = ImageSource.FromStream(() => file.GetStream());
            }

            catch (Exception ex)
            {
                await this.DisplayAlert("Erro", "Problema ao Executar ação", "ok");
            }
        }

        private void btnLimpar_Clicked(object sender, EventArgs e)
        {
            img = null;
            imgfoto.Source = null;
        }
    }
}
