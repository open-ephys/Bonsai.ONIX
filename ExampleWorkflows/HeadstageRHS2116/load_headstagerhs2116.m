%% Load all the data from HeadstageRHS2116 workflow
suffix = '2023-05-10T15_43_23'; % Change to match file names' suffix
timezone = 'America/New_York'; % Change to timezone in which recording was performed
rhs_data_type = 'float32'; % int16, uint16

% Recording start wall clock time
fid = fopen(['start-time_' suffix '.csv']);
x = textscan(fid, '%s');
start_time = datetime(x{1}{1}, 'TimeZone', timezone, 'Format', 'yyyy-MM-dd''T''HH:mm:ss.SSSSXXX');
fclose(fid);

% Headstage link status
fid = fopen(['headstage-port-status_' suffix '.csv']);
x = textscan(fid, '%s %u64 %s %s %d %s', 'Delimiter', ',');
link.time = x{1};
link.clock = x{2};
link.lock = ismember(x{3}, 'True');
link.pass = ismember(x{4}, 'True');
link.code = x{5};
link.message = x{6};
fclose(fid);

% RHS2116 16-Channel amplifier/stimulator chip A
fid = fopen(['rhs2116-clock_a_' suffix '.raw']);
rhs2116_a.clock = fread(fid, Inf, 'uint64');
fclose(fid);

fid = fopen(['rhs2116-hub-clock_a_' suffix '.raw']);
rhs2116_a.hub_clock = fread(fid, Inf, 'uint64');
fclose(fid);

fid = fopen(['rhs2116-ephys_a_' suffix '.raw']);
rhs2116_a.ephys = reshape(fread(fid, Inf, 'float32'), 16, [])';
fclose(fid);

fid = fopen(['rhs2116-dc_a_' suffix '.raw']);
rhs2116_a.dc = reshape(fread(fid, Inf, 'float32'), 16, [])';
fclose(fid);

% RHS2116 16-Channel amplifier/stimulator chip B
fid = fopen(['rhs2116-clock_b_' suffix '.raw']);
rhs2116_b.clock = fread(fid, Inf, 'uint64');
fclose(fid);

fid = fopen(['rhs2116-hub-clock_b_' suffix '.raw']);
rhs2116_b.hub_clock = fread(fid, Inf, 'uint64');
fclose(fid);

fid = fopen(['rhs2116-ephys_b_' suffix '.raw']);
rhs2116_b.ephys = reshape(fread(fid, Inf, 'float32'), 16, [])';
fclose(fid);

fid = fopen(['rhs2116-dc_b_' suffix '.raw']);
rhs2116_b.dc = reshape(fread(fid, Inf, 'float32'), 16, [])';
fclose(fid);

%% Cleanup temp
clearvars -except link rhs2116_a rhs2116_b start_time

%% Do some plots
close all

figure()

ax1 = subplot(211);
hold all
plot(rhs2116_a.clock, rhs2116_a.ephys)
plot(rhs2116_b.clock, rhs2116_b.ephys)

ax2 = subplot(212);
hold all
plot(rhs2116_a.clock, rhs2116_a.dc)
plot(rhs2116_b.clock, rhs2116_b.dc)

linkaxes([ax1, ax2], 'x');
