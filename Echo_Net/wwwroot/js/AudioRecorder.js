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
            mCaller.invokeMethodAsync('OnAudioUrl', pAudioUrl);
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
    

    BlazorAudioRecorder.getAudioDataFromBlobUrl = async function () {
        const reader = new FileReader();
        reader.readAsDataURL(blobObject);
        reader.addEventListener(
            "load",
            () => {
                console.log(reader.result);
                mCaller.invokeMethodAsync('OnAudioLoadRead', reader.result);
            },
            false
        );
    };
     
})();
