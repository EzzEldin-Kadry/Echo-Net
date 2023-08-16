var BlazorAudioRecorder = {};


(function () {
    var mStream;
    var mAudioChunks;
    var mMediaRecorder;
    var mCaller;
    var blobObject;
    BlazorAudioRecorder.Initialize = function (vCaller) {
        mCaller = vCaller;
    };    


    BlazorAudioRecorder.StartRecord = async function () {
        mStream = await navigator.mediaDevices.getUserMedia({ audio: true });
        mMediaRecorder = new MediaRecorder(mStream);
        mMediaRecorder.addEventListener('dataavailable', vEvent => {
        mAudioChunks.push(vEvent.data);
        });            

        mMediaRecorder.addEventListener('stop', () => {
            var pAudioBlob = new Blob(mAudioChunks, { type: "audio/wav" });
            blobObject = pAudioBlob;
            var pAudioUrl = URL.createObjectURL(pAudioBlob);
            mCaller.invokeMethodAsync('OnAudioUrlSent', pAudioUrl);
        });
        
        mAudioChunks = [];
        mMediaRecorder.start();
    };

    BlazorAudioRecorder.PauseRecord = function () {
        mMediaRecorder.pause();
    };

    BlazorAudioRecorder.ResumeRecord = function () {
        mMediaRecorder.resume();
    };

    BlazorAudioRecorder.StopRecord = function () {
        mMediaRecorder.stop();
        mStream.getTracks().forEach(pTrack => pTrack.stop());
        mAudioChunks = [];
    };
    BlazorAudioRecorder.SendAudioDataFromBlobUrl = async function () {
        const reader = new FileReader();
        reader.readAsDataURL(blobObject);
        reader.addEventListener("load", () => {
            HandleAudioUpload(reader);
        }, false);
    };

    function HandleAudioUpload(reader) {
        const audioData = reader.result;
        const chunkSize = 30 * 1024; // Set the chunk size to 30KB
        for (let i = 0; i < audioData.length; i += chunkSize) {
          const chunk = audioData.slice(i, i + chunkSize);
          mCaller.invokeMethodAsync('OnLoadAudioChunks', chunk);
        }
        mCaller.invokeMethodAsync('OnAudioDataSent', audioData.length);
      };

})();
