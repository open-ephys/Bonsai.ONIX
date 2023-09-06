import numpy as np
import matplotlib.pyplot as plt

#%% Load all the data from NeuropixelsV1e workflow

suffix = '2023-09-06T11_47_53'; # Change to match file names' suffix

# Neuropixels V1 Probe
npix = {}
npix['frame-counter'] = np.fromfile('frame-counter_' + suffix + '.raw', dtype=np.int32)

npix['spike'] = np.fromfile('spike_' + suffix + '.raw', dtype=np.uint16).reshape(-1, 384)
npix['spike-clock'] = np.fromfile('spike-clock_' + suffix + '.raw', dtype=np.uint64)

npix['lfp'] = np.fromfile('lfp_' + suffix + '.raw', dtype=np.uint16).reshape(-1, 384)
npix['lfp-clock'] = np.fromfile('lfp-clock_' + suffix + '.raw', dtype=np.uint64)


#%% Ensure Counters are correct

plt.close('all')

ax = plt.subplot(3,2,1)
ax.plot(npix['frame-counter'])

ax = plt.subplot(3,2,2)
ax.plot(np.diff(npix['frame-counter']))

ax = plt.subplot(3,2,3)
ax.plot(npix['spike-clock'])

ax = plt.subplot(3,2,4)
ax.plot(np.diff(npix['spike-clock']))

ax = plt.subplot(3,2,5)
ax.plot(npix['lfp-clock'], 'k.')

ax = plt.subplot(3,2,6)
ax.plot(np.diff(npix['lfp-clock']))


if ~np.all(np.diff(npix['frame-counter']) == 1):
    print("ERROR: Skipped frame detected.")
    
print('Spike sample rate: {}'.format(250e6 / np.mean(np.diff(npix['spike-clock']))))
print('LFP sample rate: {}'.format(250e6 / np.mean(np.diff(npix['lfp-clock']))))


#%% 

plt.close('all')
plt.plot(npix['spike'])




