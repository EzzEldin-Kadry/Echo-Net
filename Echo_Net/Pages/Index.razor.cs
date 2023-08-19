using Microsoft.JSInterop;
using Echo_Net.Models.Dto;

namespace Echo_Net.Pages
{
    public partial class Index
    {
        AudioPostState audioPostState;
        string? projectPath;
        List<AudioPostDto>? AudioPosts { get; set; }
        AudioRecorderManager audioRecorderManager;
        protected override Task OnInitializedAsync()
        {  
            projectPath = new DirectoryInfo(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location))
                        .Parent.Parent.Parent.FullName;
            InitializeAudioRecorder();
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await mJS.InvokeVoidAsync("BlazorAudioRecorder.Initialize", DotNetObjectReference.Create(this));

                GetAudioPostsFromService();
            }
        }
        void InitializeAudioRecorder()
        {
            InitializeAudioRecorderData();
            SetAudioRecorderUIElementsToDefault();
            SetAudioDataDetailsToDefault();
        }
        async void GetAudioPostsFromService()
        {
            var response = await _audioPostService.GetAllAudioPostsAsync<ResponseDto>();
            if (response is not null && response.IsSuccess)
            {
                AudioPosts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AudioPostDto>>(Convert.ToString(response.Result));
                await InvokeAsync(() => StateHasChanged());
                return;
            }
        }

        public void InitializeAudioRecorderData()
        {
            audioRecorderManager.audioBloblURL = string.Empty;
            audioRecorderManager.audioFileName = string.Empty;
            audioRecorderManager.audioExtension = ".wav";
            audioPostState = AudioPostState.NoPost;
        }

        void SetAudioRecorderUIElementsToDefault()
        {
            audioRecorderManager.mDisableRecordAudioStart = false;
            audioRecorderManager.mDisableRecordAudioPause = true;
            audioRecorderManager.mDisableRecordAudioResume = true;
            audioRecorderManager.mDisableRecordAudioStop = true;
            audioRecorderManager.mDisableRecordAudioSave = true;
        }
        void BeforeSaveStateRecordAudio()
        {
            audioRecorderManager.mDisableRecordAudioStart = false;
            audioRecorderManager.mDisableRecordAudioPause = true;
            audioRecorderManager.mDisableRecordAudioResume = true;
            audioRecorderManager.mDisableRecordAudioStop = true;
            audioRecorderManager.mDisableRecordAudioSave = false;
        }

        void SetAudioDataDetailsToDefault()
        {
            audioRecorderManager.audioDataBase64 = string.Empty;
        }

        void StartAudioRecord()
        {
            audioRecorderManager.mDisableRecordAudioStart = true;
            audioRecorderManager.mDisableRecordAudioPause = false;
            audioRecorderManager.mDisableRecordAudioResume = true;
            audioRecorderManager.mDisableRecordAudioStop = false;
            audioRecorderManager.mDisableRecordAudioSave = true;
            mJS.InvokeVoidAsync("BlazorAudioRecorder.StartRecord");
        }

        void PauseAudioRecord()
        {
            audioRecorderManager.mDisableRecordAudioStart = true;
            audioRecorderManager.mDisableRecordAudioPause = true;
            audioRecorderManager.mDisableRecordAudioResume = false;
            audioRecorderManager.mDisableRecordAudioStop = false;
            audioRecorderManager.mDisableRecordAudioSave = true;
            mJS.InvokeVoidAsync("BlazorAudioRecorder.PauseRecord");
        }

        void ResumeAudioRecord()
        {
            audioRecorderManager.mDisableRecordAudioStart = true;
            audioRecorderManager.mDisableRecordAudioPause = false;
            audioRecorderManager.mDisableRecordAudioResume = true;
            audioRecorderManager.mDisableRecordAudioStop = false;
            audioRecorderManager.mDisableRecordAudioSave = true;
            mJS.InvokeVoidAsync("BlazorAudioRecorder.ResumeRecord");
        }

        void StopAudioRecord()
        {
            BeforeSaveStateRecordAudio();
            mJS.InvokeVoidAsync("BlazorAudioRecorder.StopRecord");
            audioRecorderManager.audioFileName = "MyEcho_" + (new Random()).Next() + Constants.DateTimeStamp();
        }

        async Task SaveAudioRecord()
        {
            await mJS.InvokeVoidAsync("BlazorAudioRecorder.SendAudioDataFromBlobUrl");
            SetAudioDataDetailsToDefault();
            SetAudioRecorderUIElementsToDefault();
            InitializeAudioRecorderData();
            if(audioPostState == AudioPostState.FailedPost)
            {
                await mJS.InvokeVoidAsync("alert", "Failed to post!");
                audioPostState = AudioPostState.NoPost;
                return;
            }
            StateHasChanged();
            await mJS.InvokeVoidAsync("alert", "Success post");
            audioPostState = AudioPostState.NoPost;
        }
        

        public void Dispose()
        {
            mJS.InvokeVoidAsync("BlazorAudioRecorder.StopRecord");
        }

        [JSInvokable]
        public async Task OnAudioUrlSent(string vUrl)
        {
            audioRecorderManager.audioBloblURL = vUrl;
            await InvokeAsync(() => StateHasChanged());
        }

        [JSInvokable]
        public void LoadAudioChunks(string audioChunk)
        {
            audioRecorderManager.audioDataBase64 += audioChunk;
        }

        [JSInvokable]
        public async Task OnAudioDataSent(int expectedLengthOfAudioData)
        {
            if (audioRecorderManager.audioDataBase64.Length != expectedLengthOfAudioData)
            {
                audioPostState = AudioPostState.FailedPost;
                return;
            }
            try
            {
                byte[] audioData = Convert.FromBase64String(audioRecorderManager.audioDataBase64.Split(",")[1]);
                string filePath = Path.Combine(Constants.EchoesLocation, 
                    audioRecorderManager.audioFileName + audioRecorderManager.audioExtension);
                string fullPath = Path.Combine(projectPath, filePath);
                using (MemoryStream memoryStream = new MemoryStream(audioData))
                {
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await memoryStream.CopyToAsync(fileStream);
                        audioPostState = AudioPostState.SuccessPost;
                    }
                }
            }
            catch (Exception ex)
            {
                await mJS.InvokeVoidAsync("alert", $"Error posting audio post: {ex.Message}");
                audioPostState = AudioPostState.FailedPost;
            }
        }


    }
    struct AudioRecorderManager
    {
            public string audioBloblURL;
            public string audioFileName;
            public string audioExtension;
            public string audioDataBase64;
            public bool mDisableRecordAudioStart;
            public bool mDisableRecordAudioPause;
            public bool mDisableRecordAudioResume;
            public bool mDisableRecordAudioStop;
            public bool mDisableRecordAudioSave;
    }

    enum AudioPostState
    {
        NoPost,
        SuccessPost,
        FailedPost
    }

}