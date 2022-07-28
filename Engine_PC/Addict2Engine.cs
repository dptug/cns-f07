using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Engine_PC;

public class Addict2Engine : Game
{
	private GraphicsDeviceManager graphics;

	private ContentManager content;

	public static GraphicsDevice device;

	public static Addict2Project project;

	public Addict2Timer timer;

	public static Addict2Logger log;

	public static VertexDeclaration decl;

	private AudioEngine audioEngine;

	private WaveBank waveBank;

	private SoundBank soundBank;

	private int renderedframe;

	private long lastframe;

	public Addict2Engine()
	{
		graphics = new GraphicsDeviceManager(this);
		content = new ContentManager(base.Services);
		base.IsFixedTimeStep = false;
	}

	protected override void Initialize()
	{
		device = graphics.GraphicsDevice;
		string path = Path.Combine(StorageContainer.TitleLocation, "data\\function.min");
		FileStream s = File.Open(path, FileMode.Open);
		log = new Addict2Logger(content, device);
		graphics.PreferredBackBufferWidth = 1024;
		graphics.PreferredBackBufferHeight = 768;
		graphics.ApplyChanges();
		audioEngine = new AudioEngine("data\\function.xgs");
		waveBank = new WaveBank(audioEngine, "data\\function.xwb");
		soundBank = new SoundBank(audioEngine, "data\\function.xsb");
		decl = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
		project = new Addict2Project();
		project.XRes = graphics.PreferredBackBufferWidth;
		project.YRes = graphics.PreferredBackBufferHeight;
		project.Load(new Addict2BinaryReader(s));
		GC.Collect();
		timer = new Addict2Timer();
		soundBank.PlayCue("function_mastered");
		base.Initialize();
	}

	protected override void LoadGraphicsContent(bool loadAllContent)
	{
	}

	protected override void UnloadGraphicsContent(bool unloadAllContent)
	{
		if (unloadAllContent)
		{
			content.Unload();
		}
	}

	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
		{
			Exit();
		}
		if (Keyboard.GetState().IsKeyDown(Keys.Escape))
		{
			Exit();
		}
		audioEngine.Update();
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		device.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1f, 0);
		int frame = timer.GetTime() / 10;
		long ticks = DateTime.UtcNow.Ticks;
		Addict2Logger.Append("Current frame = " + frame + "\n");
		if (ticks - lastframe > 0)
		{
			Addict2Logger.Append("Current framerate = " + 10000000f / (float)(ticks - lastframe) + " FPS\n");
		}
		Addict2Logger.Append("Total memory used = " + GC.GetTotalMemory(forceFullCollection: false));
		project.DisplayFrame(frame);
		log.PrintLog();
		lastframe = ticks;
		base.Draw(gameTime);
	}
}
