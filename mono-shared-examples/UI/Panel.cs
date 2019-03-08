using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono_shared_examples
{
	/// <summary>
	/// Draws a panel to the screen using a tile based texture.
	/// The texture is expected to have 9 tiles arranged 3x3 as a small panel.
	/// So the top-left corner tile is to the top left and the top-right corner
	/// tile is on the top right etc.
	/// The constructor automatically figures out the tile size as one third of
	/// the texture width.  Because of this the texture needs to be exclusively
	/// dedicated to the particular panel look and not residing on a general
	/// texture used for other purposes also.
	/// Panels can be set to any size down to the pixel and should render correctly
	/// even though they are not divisible by full tile sizes.
	/// </summary>
	public class Panel
	{
		Texture2D m_tileTx;
		int m_tileSize;
		Vector2 m_pos;
		int m_width;
		int m_height;

		Rectangle m_srcCornerTl;
		Rectangle m_srcCornerTr;
		Rectangle m_srcCornerBr;
		Rectangle m_srcCornerBl;

		Rectangle m_srcHorzTop;
		Rectangle m_srcHorzBot;
		Rectangle m_srcVertLeft;
		Rectangle m_srcVertRight;

		Rectangle m_srcCenter;

		//*******************************************************************************************
		public Panel(Texture2D tileTx,int x,int y,int width,int height)
		{
			m_tileTx=tileTx;
			m_tileSize=tileTx.Width/3;
			m_pos=new Vector2(x,y);
			m_width=width;
			m_height=height;

			Init();
		}
		//*******************************************************************************************
		void Init()
		{
			int ts=m_tileSize;
			m_srcCornerTl=new Rectangle(0,0,ts,ts);
			m_srcCornerTr=new Rectangle(ts*2,0,ts,ts);
			m_srcCornerBr=new Rectangle(ts*2,ts*2,ts,ts);
			m_srcCornerBl=new Rectangle(0,ts*2,ts,ts);

			m_srcHorzTop=new Rectangle(ts,0,ts,ts);
			m_srcHorzBot=new Rectangle(ts,ts*2,ts,ts);

			m_srcVertLeft=new Rectangle(0,ts,ts,ts);
			m_srcVertRight=new Rectangle(ts*2,ts,ts,ts);

			m_srcCenter=new Rectangle(ts,ts,ts,ts);
		}
		//*******************************************************************************************
		public void Draw(SpriteBatch sb)
		{
			int horz=m_width/m_tileSize-1;
			int vert=m_height/m_tileSize-1;

			// When width or height are not evenly divisible by the tile size
			// then we will need to draw an extra tile in the given direction.
			if(m_width%m_tileSize>0){horz++;}
			if(m_height%m_tileSize>0){vert++;}

			int rightX=m_width-m_tileSize;
			int bottomY=m_height-m_tileSize;

			sb.Begin();

			// Draw center area
			for(int y=1; y<vert; y++)
			{
				for(int x=1; x<horz; x++)
				{
					Draw(sb,x*m_tileSize,y*m_tileSize,m_srcCenter);
				}
			}

			// Top and bottom horizontals
			for(int x=1; x<horz; x++)
			{
				Draw(sb,x*m_tileSize,0,m_srcHorzTop);
				Draw(sb,x*m_tileSize,bottomY,m_srcHorzBot);
			}

			// Left and right verticals
			for(int y=1; y<vert; y++)
			{
				Draw(sb,0,y*m_tileSize,m_srcVertLeft);
				Draw(sb,rightX,y*m_tileSize,m_srcVertRight);
			}

			// Draw corners
			Draw(sb,0,0,m_srcCornerTl);
			Draw(sb,m_width-m_tileSize,0,m_srcCornerTr);
			Draw(sb,m_width-m_tileSize,m_height-m_tileSize,m_srcCornerBr);
			Draw(sb,0,m_height-m_tileSize,m_srcCornerBl);
			
			sb.End();
		}
		//*******************************************************************************************
		void Draw(SpriteBatch sb,int xOff,int yOff,Rectangle srcRect)
		{
			sb.Draw(m_tileTx,m_pos+new Vector2(xOff,yOff),srcRect,Color.White);
		}
		//*******************************************************************************************
	}
}
