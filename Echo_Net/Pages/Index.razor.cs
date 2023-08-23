using Microsoft.JSInterop;
using Echo_Net.Models.Dto;

namespace Echo_Net.Pages
{
    public partial class Index
    {
        AudioPostState audioPostState;
        List<AudioPostDto>? AudioPosts { get; set; }
        AudioRecorderManager audioRecorderManager;
        protected override Task OnInitializedAsync()
        {  
            InitializeAudioRecorder();
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await mJS.InvokeVoidAsync("BlazorAudioRecorder.Initialize", DotNetObjectReference.Create(this));
                await GetAudioPostsFromService();
                await InvokeAsync(() => StateHasChanged());
            }
        }
        void InitializeAudioRecorder()
        {
            SetAudioManagerAndPostStateToDefault();
            SetAudioRecorderUIElementsToDefault();
            SetAudioDataDetailsToDefault();
            
        }
        async Task GetAudioPostsFromService()
        {
            var response = await _audioPostService.GetAllAudioPostsAsync<ResponseDto>();
            if (response is not null && response.IsSuccess)
            {
                AudioPosts = Newtonsoft.Json.JsonConvert
                    .DeserializeObject<List<AudioPostDto>>(Convert.ToString(response.Result));
                return;
            }
        }
        async Task<bool> CreateAudioPostToService(string audioURL, string title = "dummyTitle", string description = "dumyDescription")
        {
            var audioPostDto = new AudioPostDto(title, description, audioURL);
            var response = await _audioPostService.CreateAudioPostAsync<ResponseDto>(audioPostDto);
            if (response is not null && response.IsSuccess)
            {
                AudioPosts.Add(audioPostDto);
                await InvokeAsync(() => StateHasChanged());
                return true;
            }
            return false;
        }

        public void SetAudioManagerAndPostStateToDefault()
        {
            audioRecorderManager.audioBloblURL = string.Empty;
            audioRecorderManager.audioFileName = string.Empty;
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
            StateHasChanged();
        }

        async Task SaveAudioRecord()
        {
            await mJS.InvokeVoidAsync("BlazorAudioRecorder.SendAudioDataFromBlobUrl");
        }
        

        public void Dispose()
        {
            mJS.InvokeVoidAsync("BlazorAudioRecorder.StopRecord");
        }
    
        [JSInvokable]
        public void OnAudioUrlSent(string vUrl)
        {
            audioRecorderManager.audioBloblURL = vUrl;
            StateHasChanged();
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
                string filePath = AudioPostDto.ConstructFilePath(audioRecorderManager.audioFileName);
                string fullPath = Path.Combine(Environment.WebRootPath, filePath);
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

        [JSInvokable]
        public async Task OnAudioSentFinish()
        {
            if(audioPostState == AudioPostState.NoPost)
            {
                await mJS.InvokeVoidAsync("alert", "Something went wrong!");
                return;
            }
            if(audioPostState == AudioPostState.FailedPost)
            {
                await mJS.InvokeVoidAsync("alert", "Failed to upload audio record!");
                InitializeAudioRecorder();
                await InvokeAsync(() => StateHasChanged());
                return;
            }
            bool isResponseTrue = await CreateAudioPostToService(audioRecorderManager.audioFileName);
            if(!isResponseTrue)
            {
                await mJS.InvokeVoidAsync("alert", "Failed to create audio post!");
                InitializeAudioRecorder();
                await InvokeAsync(() => StateHasChanged());
                return;
            }
            await mJS.InvokeVoidAsync("alert", "Successful post");
            InitializeAudioRecorder();
            await InvokeAsync(() => StateHasChanged());
        }


    }
    struct AudioRecorderManager
    {
            public string audioBloblURL;
            public string audioFileName;
            public string audioDataBase64;
            
            public bool mDisableRecordAudioStart;
            public bool mDisableRecordAudioPause;
            public bool mDisableRecordAudioResume;
            public bool mDisableRecordAudioStop;
            public bool mDisableRecordAudioSave;

            public IWebHostEnvironment hostEnvironment;
    }

    enum AudioPostState
    {
        NoPost,
        SuccessPost,
        FailedPost
    }

}